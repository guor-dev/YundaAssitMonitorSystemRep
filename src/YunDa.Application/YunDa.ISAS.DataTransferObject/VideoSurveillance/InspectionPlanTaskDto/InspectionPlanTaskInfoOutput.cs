using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto
{
    public class InspectionPlanTaskInfoOutput : EntityDto<Guid>
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

        public virtual InspectionCardProperty InspectionCard { get; set; }
        public virtual IEnumerable<InspectionItemProperty> InspectionItems { get; set; }
    }
}