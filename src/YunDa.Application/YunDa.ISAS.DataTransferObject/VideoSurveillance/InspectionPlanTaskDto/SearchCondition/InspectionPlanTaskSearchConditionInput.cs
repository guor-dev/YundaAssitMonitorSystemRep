using System;
using System.ComponentModel;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto
{
    public class InspectionPlanTaskSearchConditionInput
    {
        public virtual string PlanTaskName { get; set; }

        public virtual Guid? InspectionCardId { get; set; }

        [DefaultValue(false)]
        public virtual bool IsNeedParent { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}