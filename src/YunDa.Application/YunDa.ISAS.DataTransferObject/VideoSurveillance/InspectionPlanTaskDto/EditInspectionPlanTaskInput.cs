using Abp.AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto
{
    [AutoMapTo(typeof(InspectionPlanTask))]
    public class EditInspectionPlanTaskInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 任务项名称
        /// </summary>
        [StringLength(InspectionPlanTask.MaxNameLength)]
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
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(InspectionPlanTask.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 所属任务单
        /// </summary>
        public virtual Guid? InspectionCardId { get; set; }
    }
}