using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.VideoSurveillance
{
    /// <summary>
    /// 视频监控终端（摄像头）预置点
    /// </summary>
    [Table("vs_preset_point")]
    public class PresetPoint : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 预制点号
        /// </summary>
        [Required]
        public virtual int Number { get; set; }

        /// <summary>
        /// 预制名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; } = true;

        public virtual Guid? VideoDevId { get; set; }

        [ForeignKey("VideoDevId")]
        public virtual VideoDev VideoDev { get; set; }

        public virtual IEnumerable<InspectionItem> InspectionItems { get; set; }
    }
}