using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Entities.GeneralInformation
{
    /// <summary>
    /// 变电站
    /// </summary>
    [Table("gi_transformer_substation")]
    public class TransformerSubstation : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxCommMgrIPLength = 20;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 变电站名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string SubstationName { get; set; }

        /// <summary>
        /// 负责一个工程内所有变电站的通信
        /// </summary>
        [StringLength(MaxCommMgrIPLength)]
        public virtual string CommMgrIP { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual Guid? PowerSupplyLineId { get; set; }

        [ForeignKey("PowerSupplyLineId")]
        public virtual PowerSupplyLine PowerSupplyLine { get; set; }

        public virtual IEnumerable<VideoDev> VideoDevs { get; set; }

        public virtual IEnumerable<InspectionCard> InspectionCards { get; set; }
    }
}