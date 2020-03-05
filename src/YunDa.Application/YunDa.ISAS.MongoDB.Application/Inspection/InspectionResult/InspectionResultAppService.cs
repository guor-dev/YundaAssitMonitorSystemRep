using Abp.Authorization;
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
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionResultDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionResultDto.SearchCondition;
using YunDa.ISAS.Entities.MongoDB;
using YunDa.ISAS.MongoDB.Repositories;

namespace YunDa.ISAS.MongoDB.Application.Inspection
{
    /// <summary>
    /// 巡检结果接口服务
    /// </summary>
    public class InspectionResultAppService : ISASAppServiceBase, IInspectionResultAppService
    {
        /// <summary>
        /// 巡检结果仓储
        /// </summary>
        private readonly IMongoDbRepository<InspectionResult, Guid> _inspectionResultRepository;

        public InspectionResultAppService(IMongoDbRepository<InspectionResult, Guid> inspectionResultRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _inspectionResultRepository = inspectionResultRepository;
        }

        #region 巡检结果编辑

        /// <summary>
        /// 巡检结果编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<RequestResult<InspectionResultOutput>> CreateOrUpdateAsync(EditInspectionResultInput input)
        {
            if (input == null) return null;
            RequestResult<InspectionResultOutput> rst;
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

        private async Task<RequestResult<InspectionResultOutput>> CreateAsync(EditInspectionResultInput input)
        {
            RequestResult<InspectionResultOutput> rst = new RequestResult<InspectionResultOutput>();
            try
            {
                var data = ObjectMapper.Map<InspectionResult>(input);
                await _inspectionResultRepository.InsertOneAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<InspectionResultOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<InspectionResultOutput>> UpdateAsync(EditInspectionResultInput input)
        {
            RequestResult<InspectionResultOutput> rst = new RequestResult<InspectionResultOutput>();
            try
            {
                var data = await _inspectionResultRepository.FirstOrDefaultAsync(e => e.Id == input.Id).ConfigureAwait(false);
                ObjectMapper.Map(input, data);
                await _inspectionResultRepository.UpdateOneAsync(data).ConfigureAwait(false);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<InspectionResultOutput>(data);
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 巡检结果编辑

        /// <summary>
        /// 根据ID删除巡检结果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _inspectionResultRepository.DeleteOneAsync(id).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        /// <summary>
        /// 根据ids删除多条巡检项
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                foreach (var id in ids)
                {
                    await DeleteByIdAsync(id).ConfigureAwait(false);
                }

                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        /// <summary>
        /// 根据条件查询巡检结果
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        [HttpPost]
        public RequestPageResult<InspectionResultOutput> FindDatas(PageSearchCondition<InspectionResultSearchConditionInput> searchCondition)
        {
            RequestPageResult<InspectionResultOutput> rst = new RequestPageResult<InspectionResultOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _inspectionResultRepository.GetAll();
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.CardName.IsNullOrEmpty(), e => e.CardName.Contains(searchCondition.SearchCondition.CardName, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.InspectionTime.HasValue, e => e.InspectionTime == searchCondition.SearchCondition.InspectionTime);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderByDescending(e => e.InspectionTime);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<InspectionResultOutput>>(datas);
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