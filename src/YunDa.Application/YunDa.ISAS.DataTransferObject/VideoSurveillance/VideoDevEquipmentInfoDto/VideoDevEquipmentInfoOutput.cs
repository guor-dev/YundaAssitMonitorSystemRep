using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevEquipmentInfoDto
{
    [AutoMapFrom(typeof(VideoDevEquipmentInfo))]
    public class VideoDevEquipmentInfoOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 关联监控设备表
        /// </summary>
        public virtual Guid? EquipmentInfoId { get; set; }

        /// <summary>
        /// 关联视频设备表
        /// </summary>
        public virtual Guid? VideoDevId { get; set; }

        /// <summary>
        /// 所属场站
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        public virtual string Remark { get; set; }
        public EquipmentInfoProperty EquipmentInfo { get; set; }
        public VideoTerminalProperty VideoDev { get; set; }
        public virtual TransformerSubstationProperty TransformerSubstation { get; set; }
    }
}