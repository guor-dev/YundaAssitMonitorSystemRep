using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto
{
    [AutoMapFrom(typeof(ManufacturerInfo))]
    public class ManufacturerInfoProperty : EntityDto<Guid>
    {
        public virtual string ManufacturerName { get; set; }
    }
}