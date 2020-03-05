using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto
{
    [AutoMapFrom(typeof(VideoDev))]
    public class VideoTerminalProperty : EntityDto<Guid>
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? SeqNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string DevName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public virtual VideoDevTypeEnum DevType { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        public virtual Guid? ManufacturerInfoId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 摄像头占用的通道号
        /// </summary>
        public virtual int? ChannelNo { get; set; }

        /// <summary>
        /// 设备通道号
        /// </summary>
        public virtual int? DevNo { get; set; }

        /// <summary>
        /// 是否支持云台
        /// </summary>
        public virtual bool IsPTZ { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual Guid? VideoDevId { get; set; }

        /// <summary>
        /// NVR硬盘录像机
        /// </summary>
        public virtual VideoNVRProperty VideoNVR { get; set; }
    }
}