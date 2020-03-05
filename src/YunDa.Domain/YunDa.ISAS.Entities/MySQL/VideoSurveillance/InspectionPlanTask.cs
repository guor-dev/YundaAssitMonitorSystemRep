using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.VideoSurveillance
{
    /// <summary>
    /// 视频巡检计划任务
    /// </summary>
    [Table("vs_inspection_plan_task")]
    public class InspectionPlanTask : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 任务项名称
        /// </summary>
        [StringLength(MaxNameLength)]
        public virtual string PlanTaskName { get; set; }

        /// <summary>
        /// 执行周
        /// </summary>
        [Required]
        public WeekEnum ExecutionWeek { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        [Required]
        public string ExecutionTime { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 所属任务单
        /// </summary>
        public virtual Guid? InspectionCardId { get; set; }

        [ForeignKey("InspectionCardId")]
        public virtual InspectionCard InspectionCard { get; set; }
    }

    public enum WeekEnum
    {
        [Description("星期一")]
        Monday = 1,

        [Description("星期二")]
        Tuesday = 2,

        [Description("星期三")]
        Wednesday = 3,

        [Description("星期四")]
        Thursday = 4,

        [Description("星期五")]
        Friday = 5,

        [Description("星期六")]
        Saturday = 6,

        [Description("星期日")]
        Sunday = 7,
    }
}