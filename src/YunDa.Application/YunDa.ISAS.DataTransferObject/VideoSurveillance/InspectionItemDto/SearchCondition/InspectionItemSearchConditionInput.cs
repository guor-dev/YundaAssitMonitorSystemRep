using System;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto.SearchCondition
{
    public class InspectionItemSearchConditionInput
    {
        public virtual Guid? InspectionCardId { get; set; }
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}