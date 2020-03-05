using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionResultDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionResultDto.SearchCondition;

namespace YunDa.ISAS.MongoDB.Application.Inspection
{
    /// <summary>
    /// 巡检结果接口服务
    /// </summary>
    public interface IInspectionResultAppService : IAppServiceBase<InspectionResultSearchConditionInput, InspectionResultOutput, EditInspectionResultInput, Guid>
    {
    }
}