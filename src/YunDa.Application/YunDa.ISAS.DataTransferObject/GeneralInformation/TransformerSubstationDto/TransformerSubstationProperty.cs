using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto
{
    [AutoMapFrom(typeof(TransformerSubstation))]
    public class TransformerSubstationProperty : EntityDto<Guid>
    {
        /// 变电站名称
        /// </summary>
        public virtual string SubstationName { get; set; }
    }
}