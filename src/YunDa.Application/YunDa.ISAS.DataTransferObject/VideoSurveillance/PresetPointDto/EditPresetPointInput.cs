using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto
{
    [AutoMapTo(typeof(PresetPoint))]
    public class EditPresetPointInput : ISASAuditedEntityDto
    {
        [Required]
        public virtual int Number { get; set; }

        /// <summary>
        /// 预制名称
        /// </summary>
        [Required]
        [StringLength(PresetPoint.MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(PresetPoint.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; } = true;

        public virtual Guid? VideoDevId { get; set; }
    }
}