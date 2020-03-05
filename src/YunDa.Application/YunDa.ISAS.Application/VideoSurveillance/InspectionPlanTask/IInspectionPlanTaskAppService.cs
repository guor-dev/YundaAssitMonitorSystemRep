using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public interface IInspectionPlanTaskAppService : IAppServiceBase<InspectionPlanTaskSearchConditionInput, InspectionPlanTaskOutput, EditInspectionPlanTaskInput, Guid>
    {
        /// <summary>
        /// 根据变电所ID获取巡检任务信息
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        RequestPageResult<InspectionPlanTaskInfoOutput> GetInspectionPlanTaskInfosBySubId(Guid? subId);

        /// <summary>复制删除数据
        /// </summary>
        /// <param name="ids">ID列表</param>
        /// <returns>返回是否复制成功</returns>
        Task<RequestEasyResult> CopyTaskByIdsAsync(List<Guid> ids);
    }
}