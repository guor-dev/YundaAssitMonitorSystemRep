using System;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevEquipmentInfoDto
{
    public class VideoDevEquipmentInfoSearchConditionInput
    {
        public virtual string VideoDevName { get; set; }
        public virtual string EquipmentInfoName { get; set; }

        /// <summary>
        /// 所属场站
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual Guid? EquipmentInfoId { get; set; }

        /// <summary>
        /// video设备ID
        /// </summary>
        public virtual Guid? VideoDevId { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}