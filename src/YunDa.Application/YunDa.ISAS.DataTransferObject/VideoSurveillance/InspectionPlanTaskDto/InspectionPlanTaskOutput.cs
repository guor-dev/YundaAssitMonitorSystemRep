using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto
{
    [AutoMapFrom(typeof(InspectionPlanTask))]
    public class InspectionPlanTaskOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 任务项名称
        /// </summary>
        public virtual string PlanTaskName { get; set; }

        /// <summary>
        /// 执行周
        /// </summary>
        public WeekEnum? ExecutionWeek { get; set; }

        public string ExecutionWeekText
        {
            get
            {
                if (this.ExecutionWeek.HasValue)
                {
                    return ((DescriptionAttribute)Attribute.GetCustomAttribute(typeof(WeekEnum).GetField(this.ExecutionWeek.ToString()), typeof(DescriptionAttribute))).Description;
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public string ExecutionTime { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 所属任务单
        /// </summary>
        public virtual Guid? InspectionCardId { get; set; }

        public virtual InspectionCardProperty InspectionCard { get; set; }
    }
}