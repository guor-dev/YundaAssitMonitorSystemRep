using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto
{
    [AutoMapFrom(typeof(PresetPoint))]
    public class PresetPointProperty : EntityDto<Guid>
    {
        /// <summary>
        /// 预制点号
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// 预制名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual Guid? VideoDevId { get; set; }
    }
}