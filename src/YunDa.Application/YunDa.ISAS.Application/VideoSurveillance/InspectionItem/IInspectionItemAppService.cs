using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto.SearchCondition;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public interface IInspectionItemAppService : IAppServiceBase<InspectionItemSearchConditionInput, InspectionItemOutput, EditInspectionItemInput, Guid>
    {
        /// <summary>
        /// 获取可执行的巡视动作下拉框数据
        /// </summary>
        /// <returns>返回满足条件的巡视动作</returns>
        RequestPageResult<SelectModelOutput> FindProcessActionsForSelect();
    }
}