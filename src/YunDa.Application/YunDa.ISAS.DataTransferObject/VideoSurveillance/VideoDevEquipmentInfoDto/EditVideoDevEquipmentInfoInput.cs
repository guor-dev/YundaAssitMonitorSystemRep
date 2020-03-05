using Abp.AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevEquipmentInfoDto
{
    [AutoMapTo(typeof(VideoDevEquipmentInfo))]
    public class EditVideoDevEquipmentInfoInput : ISASAuditedEntityDto
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

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(VideoDevEquipmentInfo.MaxRemarkLength)]
        public virtual string Remark { get; set; }
    }
}