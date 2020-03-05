using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.System.FunctionDto;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.Application.System
{
    public class FunctionAppService : ISASAppServiceBase, IFunctionAppService
    {
        private readonly IRepository<SysFunction, Guid> _repository;
        private LoginUserOutput _currentUser;
        private List<SelectModelOutput> _functionTypes;

        public FunctionAppService(IRepository<SysFunction, Guid> sysFunctionRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _repository = sysFunctionRepository;
            GetFunctionTypesValue();
        }

        #region 增/改

        [HttpPost]
        public async Task<RequestResult<FunctionOutput>> CreateOrUpdateAsync(EditFunctionInput input)
        {
            if (input == null) return null;
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            return input.Id != null ? await this.UpdateAsync(input).ConfigureAwait(false) : await this.CreateAsync(input).ConfigureAwait(false);
        }

        private async Task<RequestResult<FunctionOutput>> CreateAsync(EditFunctionInput input)
        {
            var rst = new RequestResult<FunctionOutput>() { Flag = false };
            try
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                var data = ObjectMapper.Map<SysFunction>(input);
                data = await _repository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<FunctionOutput>(data);
                rst.Flag = true;
            }
            catch
            {
            }
            return rst;
        }

        private async Task<RequestResult<FunctionOutput>> UpdateAsync(EditFunctionInput input)
        {
            var rst = new RequestResult<FunctionOutput>() { Flag = false };
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.ResultData = ObjectMapper.Map<FunctionOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 增/改

        #region 删

        [HttpGet]
        public Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult() { Flag = false };
            try
            {
                var datas = _repository.GetAllIncluding();
                List<Guid> guidsDeleting = new List<Guid>();
                guidsDeleting.AddRange(ids);
                for (int i = 0; i < guidsDeleting.Count(); i++)
                {
                    foreach (var data in datas)
                    {
                        if (guidsDeleting[i].Equals(data.SysFunctionId.Value) && !guidsDeleting.Contains(data.Id))
                        {
                            guidsDeleting.Add(data.Id);
                        }
                    }
                }

                await _repository.DeleteAsync(item => guidsDeleting.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 删

        #region 查

        [HttpPost]
        public RequestPageResult<FunctionOutput> FindDatas(PageSearchCondition<FunctionSearchConditionInput> searchCondition)
        {
            RequestPageResult<FunctionOutput> rst = new RequestPageResult<FunctionOutput>();
            if (searchCondition == null) return rst;
            try
            {
                IQueryable<SysFunction> datas;
                datas = _repository.GetAllIncluding();
                datas = GetfilteredData(datas, searchCondition);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy<SysFunction>(searchCondition.Sorting).ThenBy(dev => dev.SeqNo) : datas.OrderBy(dev => dev.Id);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                var rstDatas = ObjectMapper.Map<List<FunctionOutput>>(datas);
                rst.ResultDatas = rstDatas;
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<TreeModelOutput> FindFunctionTypeForTree()
        {
            RequestPageResult<TreeModelOutput> rst = new RequestPageResult<TreeModelOutput>();
            try
            {
                var resData = GetListTreeModelOutput();
                var rstDatas = ObjectMapper.Map<List<TreeModelOutput>>(resData);
                rst.ResultDatas = rstDatas;
                rst.Flag = true;
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
            return rst;
        }

        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <returns>客户端和浏览器端的树形结构</returns>
        private List<TreeModelOutput> GetListTreeModelOutput()
        {
            var datas = _repository.GetAll();
            var resData = new List<TreeModelOutput>();
            _functionTypes.ForEach(item =>
            {
                resData.Add(new TreeModelOutput()
                {
                    Id = new Guid("00000000-0000-0000-0000-00000000000" + item.Value),
                    Text = item.Text,
                    Type = item.Value,
                });
            });
            var list = datas.Select(item => new TreeModelOutput()
            {
                Id = item.Id,
                Text = item.Name,
                Type = ((int)item.Type).ToString(),
                Ico = item.Icon,
                ParentId = item.SysFunctionId,
            }).ToList();

            foreach (var item in list)
            {
                var s = list.Where(o => o.ParentId == item.Id);
                if (s != null)
                {
                    item.Children = s.ToList();
                }
            }
            resData.ForEach(item =>
            {
                var res = list.Where(sitem => sitem.ParentId == item.Id);
                if (res != null)
                    item.Children = res.ToList();
            });
            return resData;
        }

        private TreeModelOutput InsertTreeModelData(TreeModelOutput sourcetreeModel, TreeModelOutput searchtreeModel, TreeModelOutput newtreeModel)
        {
            if (sourcetreeModel.Id == searchtreeModel.Id)
            {
                if (sourcetreeModel.Children == null)
                {
                    sourcetreeModel.Children = new List<TreeModelOutput>() { newtreeModel };
                }
                else
                {
                    sourcetreeModel.Children.Add(newtreeModel);
                }
                return sourcetreeModel;
            }
            else
            {
                foreach (var item in sourcetreeModel.Children)
                {
                    return InsertTreeModelData(item, searchtreeModel, newtreeModel);
                }
            }
            return null;
        }

        private TreeModelOutput InsertTreeModelData1(TreeModelOutput sourcetreeModel, TreeModelOutput searchtreeModel, List<TreeModelOutput> child)
        {
            if (sourcetreeModel.Id == searchtreeModel.Id)
            {
                sourcetreeModel.Children = child;
                return sourcetreeModel;
            }
            else
            {
                foreach (var item in sourcetreeModel.Children)
                {
                    return InsertTreeModelData1(item, searchtreeModel, child);
                }
            }
            return null;
        }

        private IQueryable<SysFunction> GetfilteredData(IQueryable<SysFunction> datas, PageSearchCondition<FunctionSearchConditionInput> searchCondition)
        {
            //按照id查询所有父id和查询id相等的条列
            var resList = datas.WhereIf(!string.IsNullOrWhiteSpace(searchCondition.SearchCondition.Name), item => item.Name.Contains(searchCondition.SearchCondition.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(searchCondition.SearchCondition.Code), item => item.Code.Contains(searchCondition.SearchCondition.Code))
                .WhereIf(searchCondition.SearchCondition.Id.HasValue, item => item.SysFunctionId == searchCondition.SearchCondition.Id);
            return resList;
        }

        private void GetFunctionTypesValue()
        {
            _functionTypes = new List<SelectModelOutput>();
            foreach (var item in Enum.GetValues(typeof(FunctionType)).Cast<FunctionType>())
            {
                var description = GetEnumDescription(item);
                _functionTypes.Add(new SelectModelOutput()
                {
                    Key = item.ToString(),
                    Text = description,
                    Value = ((int)item).ToString(),
                });
            }
        }

        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindTypesForSelect()
        {
            RequestPageResult<SelectModelOutput> rst = new RequestPageResult<SelectModelOutput>();
            try
            {
                var datas = _functionTypes;
                var rstDatas = ObjectMapper.Map<List<SelectModelOutput>>(datas);
                rst.ResultDatas = rstDatas;
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        /// <summary>
        /// 获取枚举值上的Description特性的说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        private string GetEnumDescription<T>(T obj)
        {
            var type = obj.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, obj));
            DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }

            return descAttr.Description;
        }

        #endregion 查
    }
}