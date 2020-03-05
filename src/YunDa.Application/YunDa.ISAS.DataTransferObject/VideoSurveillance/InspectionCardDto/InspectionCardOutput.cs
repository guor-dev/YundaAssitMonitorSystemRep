using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto
{
    [AutoMapFrom(typeof(InspectionCard))]
    public class InspectionCardOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 视频巡检任务单名称
        /// </summary>
        public virtual string CardName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 所属变电所
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        public virtual TransformerSubstationProperty TransformerSubstation { get; set; }
        public virtual IEnumerable<InspectionItemOutput> InspectionItems { get; set; }
        public virtual IEnumerable<InspectionPlanTaskOutput> InspectionPlanTasks { get; set; }
    }
}