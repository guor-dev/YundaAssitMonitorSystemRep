using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public class ManufacturerInfoAppService : ISASAppServiceBase, IManufacturerInfoAppService
    {
        private IRepository<ManufacturerInfo, Guid> _manufacturerInfoRepository = default;
        private LoginUserOutput _currentUser = null;

        public ManufacturerInfoAppService(IRepository<ManufacturerInfo, Guid> manufacturerInfoRepository, ISessionAppService sessionAppService) : base(sessionAppService)
        {
            _manufacturerInfoRepository = manufacturerInfoRepository;
        }

        #region 增/改

        [HttpPost]
        public async Task<RequestResult<ManufacturerInfoOutput>> CreateOrUpdateAsync(EditManufacturerInfoInput input)
        {
            if (input == null) return null;
            RequestResult<ManufacturerInfoOutput> rst;
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            if (input.Id != null)
            {
                rst = await this.UpdateAsync(input).ConfigureAwait(false);
            }
            else
            {
                rst = await this.CreateAsync(input).ConfigureAwait(false);
            }
            return rst;
        }

        private async Task<RequestResult<ManufacturerInfoOutput>> UpdateAsync(EditManufacturerInfoInput input)
        {
            RequestResult<ManufacturerInfoOutput> rst = new RequestResult<ManufacturerInfoOutput>();
            try
            {
                var data = await _manufacturerInfoRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<ManufacturerInfoOutput>(data);
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<ManufacturerInfoOutput>> CreateAsync(EditManufacturerInfoInput input)
        {
            RequestResult<ManufacturerInfoOutput> rst = new RequestResult<ManufacturerInfoOutput>();
            try
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                var data = ObjectMapper.Map<ManufacturerInfo>(input);
                data = await _manufacturerInfoRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<ManufacturerInfoOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
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
                var data = await _manufacturerInfoRepository.FirstOrDefaultAsync(item => item.Id == id).ConfigureAwait(false);
                LoginUserOutput CurrentUser = base.GetCurrentUser();
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = CurrentUser.Id;
                data.IsDeleted = true;
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                //var datas = await _manufacturerInfoRepository.GetAllListAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                await _manufacturerInfoRepository.DeleteAsync(item => ids.Contains(item.Id));
                //LoginUserOutput CurrentUser = base.GetCurrentUser();
                //datas.ForEach(data =>
                //{
                //    data.DeletionTime = DateTime.Now;
                //    data.DeleterUserId = CurrentUser.Id;
                //    data.IsDeleted = true;
                //});
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public async Task<RequestEasyResult> SoftDeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                var datas = await _manufacturerInfoRepository.GetAllListAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                datas.ForEach(data =>
                {
                    LoginUserOutput CurrentUser = base.GetCurrentUser();
                    data.DeletionTime = DateTime.Now;
                    data.DeleterUserId = CurrentUser.Id;
                    data.IsDeleted = true;
                });
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 删

        #region 查

        [HttpPost]
        public RequestPageResult<ManufacturerInfoOutput> FindDatas(PageSearchCondition<ManufacturerInfoSearchConditionInput> searchCondition)
        {
            RequestPageResult<ManufacturerInfoOutput> rst = new RequestPageResult<ManufacturerInfoOutput>() { Flag = false };
            if (searchCondition == null) return rst;
            try
            {
                var datas = GetRepositoryData(searchCondition);
                //获取条件下所有数量
                var totalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(item => searchCondition.Sorting) : datas.OrderBy(item => item.ManufacturerName);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);

                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);

                rst.TotalCount = totalCount;
                rst.ResultDatas = ObjectMapper.Map<List<ManufacturerInfoOutput>>(datas);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindManufacturerInfoForSelect(PageSearchCondition<ManufacturerInfoSearchConditionInput> searchCondition)
        {
            var rst = new RequestPageResult<SelectModelOutput>() { Flag = false };
            if (searchCondition == null)
                return rst;
            try
            {
                var trupleData = GetRepositoryData(searchCondition);
                rst.ResultDatas = trupleData.Where(ter => ter.IsActive).Select(item => new SelectModelOutput()
                {
                    Value = item.Id.ToString().ToLower(),
                    Key = item.Id,
                    Text = item.ManufacturerName
                }).ToList();
                rst.TotalCount = trupleData.Count();
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        private IQueryable<ManufacturerInfo> GetRepositoryData(PageSearchCondition<ManufacturerInfoSearchConditionInput> searchCondition)
        {
            var datas = _manufacturerInfoRepository.GetAll();
            datas = datas.
                WhereIf(searchCondition.SearchCondition.IsOnlyDeleted, ter => ter.IsDeleted).
                WhereIf(!searchCondition.SearchCondition.IsOnlyDeleted, ter => !ter.IsDeleted).
                WhereIf(searchCondition.SearchCondition.IsOnlyActive, ter => ter.IsActive).
                WhereIf(!searchCondition.SearchCondition.ManufacturerName.IsNullOrEmpty(), item => item.ManufacturerName.Contains(searchCondition.SearchCondition.ManufacturerName, StringComparison.Ordinal));

            return datas;
        }

        #endregion 查

        #region 恢复

        [HttpPost]
        public async Task<RequestEasyResult> RecoverByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                var datas = await _manufacturerInfoRepository.GetAllListAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                datas.ForEach(data =>
                {
                    LoginUserOutput CurrentUser = base.GetCurrentUser();
                    data.LastModificationTime = DateTime.Now;
                    data.LastModifierUserId = CurrentUser.Id;
                    data.IsDeleted = false;
                    rst.Flag = true;
                });
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 恢复
    }
}