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
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto.SearchCondition;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public class InspectionItemAppService : ISASAppServiceBase, IInspectionItemAppService
    {
        private readonly IRepository<InspectionItem, Guid> _inspectionItemRepository;

        public InspectionItemAppService(IRepository<InspectionItem, Guid> inspectionItemRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _inspectionItemRepository = inspectionItemRepository;
        }

        #region 添加或更新变电所视频巡检任务清单

        [HttpPost]
        public async Task<RequestResult<InspectionItemOutput>> CreateOrUpdateAsync(EditInspectionItemInput input)
        {
            if (input == null) return null;
            RequestResult<InspectionItemOutput> rst;
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

        private async Task<RequestResult<InspectionItemOutput>> UpdateAsync(EditInspectionItemInput input)
        {
            RequestResult<InspectionItemOutput> rst = new RequestResult<InspectionItemOutput>();
            try
            {
                var data = await _inspectionItemRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<InspectionItemOutput>(data); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<InspectionItemOutput>> CreateAsync(EditInspectionItemInput input)
        {
            RequestResult<InspectionItemOutput> rst = new RequestResult<InspectionItemOutput>();
            try
            {
                var data = ObjectMapper.Map<InspectionItem>(input);
                data = await _inspectionItemRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<InspectionItemOutput>(data);
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
                await _inspectionItemRepository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
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
                await _inspectionItemRepository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<InspectionItemOutput> FindDatas(PageSearchCondition<InspectionItemSearchConditionInput> searchCondition)
        {
            RequestPageResult<InspectionItemOutput> rst = new RequestPageResult<InspectionItemOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _inspectionItemRepository.GetAllIncluding(item => item.InspectionCard, item => item.PresetPoint, item => item.VideoDev);
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(ter => ter.IsActive);
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.ItemName.IsNullOrEmpty(), item => item.ItemName.Contains(searchCondition.SearchCondition.ItemName, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.InspectionCardId.HasValue, item => item.InspectionCardId == searchCondition.SearchCondition.InspectionCardId);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting).ThenBy(item => item.SeqNo) : datas.OrderBy(item => item.SeqNo);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<InspectionItemOutput>>(datas);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        public RequestPageResult<SelectModelOutput> FindProcessActionsForSelect()
        {
            RequestPageResult<SelectModelOutput> rst = new RequestPageResult<SelectModelOutput>();
            try
            {
                rst.ResultDatas = base.GetEnumTypes<ProcessActionEnum>();
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