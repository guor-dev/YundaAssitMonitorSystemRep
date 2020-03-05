using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
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
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto.SearchCondition;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public class InspectionCardAppService : ISASAppServiceBase, IInspectionCardAppService
    {
        private readonly IRepository<InspectionCard, Guid> _inspectionCardRepository;

        public InspectionCardAppService(IRepository<InspectionCard, Guid> inspectionCardRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _inspectionCardRepository = inspectionCardRepository;
        }

        #region 添加或更新变电所视频巡检任务清单

        [HttpPost]
        public async Task<RequestResult<InspectionCardOutput>> CreateOrUpdateAsync(EditInspectionCardInput input)
        {
            if (input == null) return null;
            RequestResult<InspectionCardOutput> rst;
            LoginUserOutput CurrentUser = base.GetCurrentUser();
            if (input.Id != null)
            {
                input.LastModificationTime = DateTime.Now;
                input.LastModifierUserId = CurrentUser.Id;
                rst = await this.UpdateAsync(input).ConfigureAwait(false);
            }
            else
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = CurrentUser.Id;
                rst = await this.CreateAsync(input).ConfigureAwait(false);
            }
            return rst;
        }

        private async Task<RequestResult<InspectionCardOutput>> UpdateAsync(EditInspectionCardInput input)
        {
            RequestResult<InspectionCardOutput> rst = new RequestResult<InspectionCardOutput>();
            try
            {
                var data = await _inspectionCardRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<InspectionCardOutput>(data); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<InspectionCardOutput>> CreateAsync(EditInspectionCardInput input)
        {
            RequestResult<InspectionCardOutput> rst = new RequestResult<InspectionCardOutput>();
            try
            {
                var data = ObjectMapper.Map<InspectionCard>(input);
                data = await _inspectionCardRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<InspectionCardOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 添加或更新变电所视频巡检任务清单

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                var data = _inspectionCardRepository.GetAllIncluding(card => card.InspectionItems, card => card.InspectionPlanTasks).FirstOrDefault(card => card.Id == id);
                await _inspectionCardRepository.DeleteAsync(data).ConfigureAwait(false);
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
                await _inspectionCardRepository.DeleteAsync(card => ids.Contains(card.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public RequestPageResult<InspectionCardOutput> FindDatas(PageSearchCondition<InspectionCardSearchConditionInput> searchCondition)
        {
            RequestPageResult<InspectionCardOutput> rst = new RequestPageResult<InspectionCardOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = searchCondition.SearchCondition.IsNeedChildren ? _inspectionCardRepository.GetAllIncluding(card => card.TransformerSubstation, card => card.InspectionItems, card => card.InspectionPlanTasks) : _inspectionCardRepository.GetAllIncluding(card => card.TransformerSubstation);
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(ter => ter.IsActive);
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.CardName.IsNullOrEmpty(), card => card.CardName.Contains(searchCondition.SearchCondition.CardName, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.TransformerSubstationId.HasValue, card => card.TransformerSubstationId == searchCondition.SearchCondition.TransformerSubstationId);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderBy(card => card.CardName);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<InspectionCardOutput>>(datas);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }
    }
}