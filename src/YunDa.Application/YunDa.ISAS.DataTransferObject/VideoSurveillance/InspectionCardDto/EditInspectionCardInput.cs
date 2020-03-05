using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto
{
    [AutoMapTo(typeof(InspectionCard))]
    public class EditInspectionCardInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 视频巡检任务单名称
        /// </summary>
        [Required]
        [StringLength(InspectionCard.MaxNameLength)]
        public virtual string CardName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(InspectionCard.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; } = true;

        /// <summary>
        /// 所属变电所
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }
    }
}