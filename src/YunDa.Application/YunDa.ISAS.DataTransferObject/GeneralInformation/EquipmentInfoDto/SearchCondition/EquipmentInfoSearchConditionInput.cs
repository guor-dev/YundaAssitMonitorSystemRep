using System;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto.SearchCondition
{
    public class EquipmentInfoSearchConditionInput
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 所属设别类别
        /// </summary>
        public virtual Guid? EquipmentTypeId { get; set; }

        /// <summary>
        /// 所属变电站
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        /// <summary>
        /// 是否只查找活动的数据
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}