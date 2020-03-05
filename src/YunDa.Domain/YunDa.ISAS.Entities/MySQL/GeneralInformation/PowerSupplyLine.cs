using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.GeneralInformation
{
    /// <summary>
    /// 供电线路
    /// </summary>
    [Table("gi_power_supply_line")]
    public class PowerSupplyLine : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxRemarkLength = 200;

        [Required]
        [StringLength(MaxNameLength)]
        public virtual string LineName { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual IEnumerable<TransformerSubstation> TransformerSubstations { get; set; }
    }
}