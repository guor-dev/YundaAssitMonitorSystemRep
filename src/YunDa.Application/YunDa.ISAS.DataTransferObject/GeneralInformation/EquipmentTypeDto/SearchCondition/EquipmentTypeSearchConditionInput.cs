using System;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto.SearchCondition
{
    public class EquipmentTypeSearchConditionInput
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否只查找活动的数据
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }

        /// <summary>
        /// 车站Id
        /// </summary>
        //public virtual Guid SubstationId { get; set; }
    }
}