using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto.SearchCondition;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public class EquipmentTypeAppService : ISASAppServiceBase, IEquipmentTypeAppService
    {
        private IRepository<EquipmentType, Guid> _repository = default;
        private LoginUserOutput _currentUser;

        public EquipmentTypeAppService(IRepository<EquipmentType, Guid> repository, ISessionAppService sessionAppService) : base(sessionAppService)
        {
            _repository = repository;
            //_currentUser = base.GetCurrentUser();
        }

        #region 增/改

        [HttpPost]
        public async Task<RequestResult<EquipmentTypeOutput>> CreateOrUpdateAsync(EditEquipmentTypeInput input)
        {
            if (input == null) return null;
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            var rst = new RequestResult<EquipmentTypeOutput>() { Flag = false };
            if (VerficationExistName(ref rst, input))
                return rst;
            return input.Id != null ? await this.UpdateAsync(input).ConfigureAwait(false) : await this.CreateAsync(input).ConfigureAwait(false);
        }

        private async Task<RequestResult<EquipmentTypeOutput>> CreateAsync(EditEquipmentTypeInput input)
        {
            var rst = new RequestResult<EquipmentTypeOutput>() { Flag = false };
            try
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;

                var data = ObjectMapper.Map<EquipmentType>(input);
                data = await _repository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<EquipmentTypeOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        private async Task<RequestResult<EquipmentTypeOutput>> UpdateAsync(EditEquipmentTypeInput input)
        {
            var rst = new RequestResult<EquipmentTypeOutput>() { Flag = false };
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.ResultData = ObjectMapper.Map<EquipmentTypeOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 增/改

        #region 删

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _repository.FirstOrDefaultAsync(item => item.Id == id).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult() { Flag = false };
            try
            {
                await _repository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 删

        #region 查

        [HttpPost]
        public RequestPageResult<EquipmentTypeOutput> FindDatas(PageSearchCondition<EquipmentTypeSearchConditionInput> searchCondition)
        {
            var rst = new RequestPageResult<EquipmentTypeOutput>() { Flag = false };
            if (searchCondition == null)
                return rst;
            try
            {
                var datas = GetRepositoryData(searchCondition);
                var count = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting).ThenBy(item => item.Name) : datas.OrderBy(type => type.Name);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<EquipmentTypeOutput>>(datas);
                rst.TotalCount = count;
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindEquipmentTypeForSelect(PageSearchCondition<EquipmentTypeSearchConditionInput> searchCondition)
        {
            var rst = new RequestPageResult<SelectModelOutput>() { Flag = false };
            if (searchCondition == null)
                return rst;

            try
            {
                searchCondition.SearchCondition.IsOnlyActive = true;
                var datas = GetRepositoryData(searchCondition);
                rst.ResultDatas = datas.Select(item => new SelectModelOutput()
                {
                    Value = item.Id.ToString().ToLower(),
                    Key = item.Id,
                    Text = item.Name
                }).ToList();
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        private IQueryable<EquipmentType> GetRepositoryData(PageSearchCondition<EquipmentTypeSearchConditionInput> searchCondition)
        {
            var datas = _repository.GetAll();
            datas = datas
                .WhereIf(searchCondition.SearchCondition.IsOnlyActive, type => type.IsActive)
                .WhereIf(!string.IsNullOrWhiteSpace(searchCondition.SearchCondition.Name), type => type.Name.Contains(searchCondition.SearchCondition.Name, StringComparison.Ordinal));

            return datas;
        }

        /// <summary>
        /// 验证名称是否已经存在
        /// </summary>
        /// <param name="rst"></param>
        /// <param name="input"></param>
        private bool VerficationExistName(ref RequestResult<EquipmentTypeOutput> rst, EditEquipmentTypeInput input)
        {
            var datas = _repository.GetAll();
            var resData = datas.Where(item => item.Name == input.Name);
            if (resData.Count() > 0)
            {
                if (input.Id == null)
                {
                    rst.Message = "已经存存在相同名称的类型";
                    return true;
                }
                else
                {
                    if (input.IsActive != resData.ToArray()[0].IsActive)
                    {
                        return false;
                    }
                    else if (input.IsActive == resData.ToArray()[0].IsActive)
                    {
                        rst.Message = "未修改任何内容";
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion 查
    }
}