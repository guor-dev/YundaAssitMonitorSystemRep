using Abp.AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto
{
    [AutoMapTo(typeof(EquipmentInfo))]
    public class EditEquipmentInfoInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [Required]
        [StringLength(EquipmentInfo.MaxNameLength)]
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
        [Required]
        public virtual Guid? EquipmentTypeId { get; set; }

        /// <summary>
        /// 生产厂商
        /// </summary>
        [Required]
        public virtual Guid? ManufacturerInfoId { get; set; }

        /// <summary>
        /// 所属变电站
        /// </summary>
        [Required]
        public virtual Guid? TransformerSubstationId { get; set; }

        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        public virtual string Remark { get; set; }
    }
}