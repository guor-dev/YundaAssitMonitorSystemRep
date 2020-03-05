using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto
{
    [AutoMapTo(typeof(TransformerSubstation))]
    public class EditTransformerSubstationInput : ISASAuditedEntityDto
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
    }
}