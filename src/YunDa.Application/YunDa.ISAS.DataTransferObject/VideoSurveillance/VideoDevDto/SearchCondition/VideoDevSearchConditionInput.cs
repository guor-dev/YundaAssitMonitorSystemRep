using System;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto
{
    public class VideoDevSearchConditionInput
    {
        public virtual Guid? ManufacturerInfoId { get; set; }
        public virtual Guid? TransformerSubstationId { get; set; }
        public virtual Guid? VideoDevId { get; set; }
        public virtual string DevName { get; set; }

        /// <summary>
        /// 是否需要查找子元素
        /// </summary>
        public virtual bool IsNeedChildren { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }

        /// <summary>
        /// 是否是摄像头
        /// </summary>
        public virtual bool IsVideoTerminal { get; set; }

        /// <summary>
        /// 是否需要预置点
        /// </summary>
        public virtual bool IsNeedPresetPoint { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public virtual VideoDevTypeEnum? DevType { get; set; }
    }
}