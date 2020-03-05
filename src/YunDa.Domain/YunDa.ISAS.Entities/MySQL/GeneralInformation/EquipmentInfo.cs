using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.GeneralInformation
{
    [Table("gi_equipment_info")]
    public class EquipmentInfo : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 50;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 厂商名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
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
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [ForeignKey("EquipmentTypeId")]
        public virtual EquipmentType EquipmentType { get; set; }

        [ForeignKey("ManufacturerInfoId")]
        public virtual ManufacturerInfo ManufacturerInfo { get; set; }

        [ForeignKey("TransformerSubstationId")]
        public virtual TransformerSubstation TransformerSubstation { get; set; }
    }
}