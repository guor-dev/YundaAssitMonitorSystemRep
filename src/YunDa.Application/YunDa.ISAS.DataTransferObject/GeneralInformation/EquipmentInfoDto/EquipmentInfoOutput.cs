using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto
{
    [AutoMapFrom(typeof(EquipmentInfo))]
    public class EquipmentInfoOutput : EntityDto<Guid>
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
        /// 安装时间
        /// </summary>
        public virtual DateTime? InstallationDate { get; set; }

        /// <summary>
        /// 出厂时间
        /// </summary>
        public virtual DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 所属设别类别
        /// </summary>
        public virtual Guid? EquipmentTypeId { get; set; }

        /// <summary>
        /// 生产厂商
        /// </summary>
        public virtual Guid? ManufacturerInfoId { get; set; }

        /// <summary>
        /// 所属变电站
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        public virtual EquipmentTypeProperty EquipmentType { get; set; }
        public virtual ManufacturerInfoProperty ManufacturerInfo { get; set; }
        public virtual TransformerSubstationProperty TransformerSubstation { get; set; }
    }
}