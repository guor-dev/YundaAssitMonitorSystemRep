using System;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto.SearchCondition
{
    public class InspectionCardSearchConditionInput
    {
        /// <summary>
        /// 所属变电所
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        public virtual string CardName { get; set; }

        /// <summary>
        /// 是否需要查找子元素
        /// </summary>
        public virtual bool IsNeedChildren { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}