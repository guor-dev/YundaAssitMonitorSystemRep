using System;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto
{
    public class PresetPointSearchCondition
    {
        /// <summary>
        /// 预制点号
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// 预制名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 摄像头ID
        /// </summary>
        public virtual Guid? VideoDevId { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}