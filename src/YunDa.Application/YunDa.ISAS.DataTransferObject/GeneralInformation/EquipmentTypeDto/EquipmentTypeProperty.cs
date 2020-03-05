using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto
{
    [AutoMapFrom(typeof(EquipmentType))]
    public class EquipmentTypeProperty : EntityDto<Guid>
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string Name { get; set; }
    }
}