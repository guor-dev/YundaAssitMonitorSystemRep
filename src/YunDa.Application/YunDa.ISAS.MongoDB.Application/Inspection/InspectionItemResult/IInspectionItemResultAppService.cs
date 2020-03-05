using System;
using System.Collections.Generic;
using System.Text;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemResultDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemResultDto.SearchCondition;

namespace YunDa.ISAS.MongoDB.Application.Inspection
{
    public interface IInspectionItemResultAppService : IAppServiceBase<InspectionItemResultSearchConditionInput, InspectionItemResultOutput, EditInspectionItemResultInput, Guid>
    {
    }
}