using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto
{
    [AutoMapFrom(typeof(TransformerSubstation))]
    public class TransformerSubstationOutput : EntityDto<Guid>
    {
        /// 变电站名称
        /// </summary>
        [Required]
        [StringLength(TransformerSubstation.MaxNameLength)]
        public virtual string SubstationName { get; set; }

        /// <summary>
        /// 负责一个工程内所有变电站的通信
        /// </summary>
        [StringLength(TransformerSubstation.MaxCommMgrIPLength)]
        public virtual string CommMgrIP { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(TransformerSubstation.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual Guid? PowerSupplyLineId { get; set; }

        public virtual IEnumerable<VideoDevOutput> VideoDevs { get; set; }

        public virtual IEnumerable<InspectionCardOutput> InspectionCards { get; set; }
    }
}