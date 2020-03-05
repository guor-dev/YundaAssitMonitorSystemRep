using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto.SearchCondition;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    /// <summary>
    /// 巡检记录单接口服务
    /// </summary>
    public interface IInspectionCardAppService : IAppServiceBase<InspectionCardSearchConditionInput, InspectionCardOutput, EditInspectionCardInput, Guid>
    {
    }
}