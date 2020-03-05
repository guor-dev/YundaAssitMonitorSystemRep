using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Configuration;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemResultDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemResultDto.SearchCondition;
using YunDa.ISAS.Entities.MongoDB;
using YunDa.ISAS.MongoDB.Repositories;

namespace YunDa.ISAS.MongoDB.Application.Inspection
{
    /// <summary>
    /// 巡检项结果接口服务
    /// </summary>
    public class InspectionItemResultAppService : ISASAppServiceBase, IInspectionItemResultAppService

    {
        /// <summary>
        /// 巡检项结果仓储
        /// </summary>
        private readonly IMongoDbRepository<InspectionItemResult, Guid> _inspectionItemResultRepository;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly IAppServiceConfiguration _appServiceConfiguration;

        public InspectionItemResultAppService(IMongoDbRepository<InspectionItemResult, Guid> inspectionItemResultRepository, IAppServiceConfiguration appServiceConfiguration, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _inspectionItemResultRepository = inspectionItemResultRepository;
            _appServiceConfiguration = appServiceConfiguration;
        }

        #region 巡检结果编辑

        /// <summary>
        /// 巡检项结果编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<RequestResult<InspectionItemResultOutput>> CreateOrUpdateAsync(EditInspectionItemResultInput input)
        {
            if (input == null) return null;
            RequestResult<InspectionItemResultOutput> rst;
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

        private async Task<RequestResult<InspectionItemResultOutput>> CreateAsync(EditInspectionItemResultInput input)
        {
            RequestResult<InspectionItemResultOutput> rst = new RequestResult<InspectionItemResultOutput>();
            try
            {
                var data = ObjectMapper.Map<InspectionItemResult>(input);
                SetAttachmentFolder(ref data);
                await _inspectionItemResultRepository.InsertOneAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<InspectionItemResultOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private string SetAttachmentFolder(ref InspectionItemResult input)
        {
            string bPath = _appServiceConfiguration.SysAttachmentFolder;
            if (string.IsNullOrWhiteSpace(bPath))
                bPath = AppDomain.CurrentDomain.BaseDirectory;
            input.BasePath = bPath;
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(@"\InspectionItemResult\");
            strBuilder.Append(input.AnalysisTime.ToString("yyyy") + @"\");
            strBuilder.Append(input.AnalysisTime.ToString("MM-dd") + @"\");
            strBuilder.Append(input.InspectionResultId.ToString() + @"\");
            strBuilder.Append(input.ItemName + @"\");
            input.RelativePath = strBuilder.ToString();
            string dirPath = input.BasePath + input.RelativePath;
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return bPath;
        }

        private async Task<RequestResult<InspectionItemResultOutput>> UpdateAsync(EditInspectionItemResultInput input)
        {
            RequestResult<InspectionItemResultOutput> rst = new RequestResult<InspectionItemResultOutput>();
            try
            {
                var data = await _inspectionItemResultRepository.FirstOrDefaultAsync(e => e.Id == input.Id).ConfigureAwait(false);
                ObjectMapper.Map(input, data);
                await _inspectionItemResultRepository.UpdateOneAsync(data).ConfigureAwait(false);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<InspectionItemResultOutput>(data);
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 巡检结果编辑

        /// <summary>
        /// 根据ID删除巡检项结果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _inspectionItemResultRepository.DeleteOneAsync(id).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        /// <summary>
        /// 根据ids删除多条巡检项结果
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
        /// 根据条件查询巡检项结果
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        [HttpPost]
        public RequestPageResult<InspectionItemResultOutput> FindDatas(PageSearchCondition<InspectionItemResultSearchConditionInput> searchCondition)
        {
            RequestPageResult<InspectionItemResultOutput> rst = new RequestPageResult<InspectionItemResultOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _inspectionItemResultRepository.GetAll();
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.ItemName.IsNullOrEmpty(), e => e.ItemName.Contains(searchCondition.SearchCondition.ItemName, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.InspectionResultId.HasValue, e => e.InspectionResultId == searchCondition.SearchCondition.InspectionResultId)
                    .WhereIf(searchCondition.SearchCondition.MaxAnalysisTime.HasValue, e => e.AnalysisTime < searchCondition.SearchCondition.MaxAnalysisTime)
                    .WhereIf(searchCondition.SearchCondition.MinAnalysisTime.HasValue, e => e.AnalysisTime >= searchCondition.SearchCondition.MinAnalysisTime);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderByDescending(e => e.AnalysisTime);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<InspectionItemResultOutput>>(datas);
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