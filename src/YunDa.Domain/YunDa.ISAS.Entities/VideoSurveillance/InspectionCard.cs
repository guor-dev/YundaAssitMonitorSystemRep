using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Entities.VideoSurveillance
{
    /// <summary>
    /// 视频巡检任务单
    /// </summary>
    [Table("vs_inspection_card")]
    public class InspectionCard : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 视频巡检任务单名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string CardName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 所属变电所
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        [ForeignKey("TransformerSubstationId")]
        public virtual TransformerSubstation TransformerSubstation { get; set; }

        public virtual IEnumerable<InspectionItem> InspectionItems { get; set; }
        public virtual IEnumerable<InspectionPlanTask> InspectionPlanTasks { get; set; }
    }
}