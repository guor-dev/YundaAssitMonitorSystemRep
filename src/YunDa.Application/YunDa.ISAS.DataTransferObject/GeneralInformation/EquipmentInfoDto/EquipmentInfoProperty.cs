using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto
{
    [AutoMapFrom(typeof(EquipmentInfo))]
    public class EquipmentInfoProperty : EntityDto<Guid>
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 所属设别类别
        /// </summary>
        public virtual Guid? EquipmentTypeId { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool IsActive { get; set; }
    }
}