using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.PowerSupplyLineDto
{
    [AutoMapFrom(typeof(PowerSupplyLine))]
    public class PowerSupplyLineOutput : EntityDto<Guid>
    {
        public virtual string LineName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        public bool IsActive { get; set; }
        public virtual IEnumerable<TransformerSubstationOutput> TransformerSubstations { get; set; }
    }
}