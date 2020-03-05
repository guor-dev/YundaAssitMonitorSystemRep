using Abp.Authorization;
using Abp.Collections.Extensions;
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
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto.SearchCondition;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public class EquipmentInfoAppService : ISASAppServiceBase, IEquipmentInfoAppService
    {
        private IRepository<EquipmentInfo, Guid> _repository = default;
        private LoginUserOutput _currentUser;

        public EquipmentInfoAppService(IRepository<EquipmentInfo, Guid> repository, ISessionAppService sessionAppService) : base(sessionAppService)
        {
            _repository = repository;
            //_currentUser = base.GetCurrentUser();
        }

        #region 增/改

        [HttpPost]
        public async Task<RequestResult<EquipmentInfoOutput>> CreateOrUpdateAsync(EditEquipmentInfoInput input)
        {
            if (input == null) return null;
            // SetCurrentUserUpdateInfo(ref input);
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            return input.Id != null ? await this.UpdateAsync(input).ConfigureAwait(false) : await this.CreateAsync(input).ConfigureAwait(false);
        }

        private async Task<RequestResult<EquipmentInfoOutput>> CreateAsync(EditEquipmentInfoInput input)
        {
            var rst = new RequestResult<EquipmentInfoOutput>() { Flag = false };
            try
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                var data = ObjectMapper.Map<EquipmentInfo>(input);
                data = await _repository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<EquipmentInfoOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        private async Task<RequestResult<EquipmentInfoOutput>> UpdateAsync(EditEquipmentInfoInput input)
        {
            var rst = new RequestResult<EquipmentInfoOutput>() { Flag = false };
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.ResultData = ObjectMapper.Map<EquipmentInfoOutput>(data);
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
                await _repository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
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
        public RequestPageResult<EquipmentInfoOutput> FindDatas(PageSearchCondition<EquipmentInfoSearchConditionInput> searchCondition)
        {
            var rst = new RequestPageResult<EquipmentInfoOutput>() { Flag = false };
            if (searchCondition == null)
                return rst;

            try
            {
                var datas = GetRepositoryData(searchCondition);
                var count = datas.Count();
                datas = string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(type => type.Name) : datas.OrderBy(searchCondition.Sorting).ThenBy(item => item.Name);
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<EquipmentInfoOutput>>(datas);
                rst.TotalCount = count;
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindEquipmentInfoForSelect(PageSearchCondition<EquipmentInfoSearchConditionInput> searchCondition)
        {
            var rst = new RequestPageResult<SelectModelOutput>() { Flag = false };
            if (searchCondition == null)
                return rst;

            try
            {
                searchCondition.SearchCondition.IsOnlyActive = true;
                var datas = GetRepositoryData(searchCondition);
                rst.ResultDatas = datas.Where(ter => ter.IsActive).Select(item => new SelectModelOutput()
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

        private IQueryable<EquipmentInfo> GetRepositoryData(PageSearchCondition<EquipmentInfoSearchConditionInput> searchCondition)
        {
            var datas = _repository.GetAllIncluding(type => type.EquipmentType, sub => sub.TransformerSubstation, mf => mf.ManufacturerInfo);
            datas = datas.
                WhereIf(searchCondition.SearchCondition.TransformerSubstationId != null, item => item.TransformerSubstationId == searchCondition.SearchCondition.TransformerSubstationId).
                WhereIf(searchCondition.SearchCondition.EquipmentTypeId != null, item => item.EquipmentTypeId == searchCondition.SearchCondition.EquipmentTypeId).
                WhereIf(searchCondition.SearchCondition.IsOnlyActive, item => item.IsActive).
                WhereIf(!string.IsNullOrWhiteSpace(searchCondition.SearchCondition.Name), item => item.Name.Contains(searchCondition.SearchCondition.Name, StringComparison.Ordinal));

            return datas;
        }

        [HttpGet]
        public async Task<RequestPageResult<EquipmentInfoOutput>> FindEquipmentById(Guid id)
        {
            RequestPageResult<EquipmentInfoOutput> rst = new RequestPageResult<EquipmentInfoOutput>();
            if (id == null || id == default) return rst;
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
                rst.ResultDatas = new List<EquipmentInfoOutput>(1) { ObjectMapper.Map<EquipmentInfoOutput>(data) };
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        /// <summary>
        /// 验证是否存在相同设备
        /// </summary>
        /// <param name="rst"></param>
        /// <param name="input"></param>
        private bool VerficationExist(ref RequestResult<EquipmentInfoOutput> rst, EditEquipmentInfoInput input)
        {
            var trupleData = GetRepositoryData(new PageSearchCondition<EquipmentInfoSearchConditionInput> { SearchCondition = new EquipmentInfoSearchConditionInput() { Name = input.Name } });
            if (trupleData.Count() > 0)
            {
                rst.Message = ResultMsgConst.OperateFail_ExistName;
                return false;
            }
            return true;
        }

        #endregion 查
    }
}