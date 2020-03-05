using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.GeneralInformation
{
    [Table("gi_equipment_type")]
    public class EquipmentType : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 50;

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 是否已经选择
        /// </summary>
        //public virtual bool IsSelected { get; set; }
    }
}