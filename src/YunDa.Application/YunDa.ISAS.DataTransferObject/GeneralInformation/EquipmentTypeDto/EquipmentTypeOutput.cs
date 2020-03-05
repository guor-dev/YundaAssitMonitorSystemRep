using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto
{
    [AutoMapFrom(typeof(EquipmentType))]
    public class EquipmentTypeOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool IsActive { get; set; }

        // public virtual string Remark { get; set; }
    }
}