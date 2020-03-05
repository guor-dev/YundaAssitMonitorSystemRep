using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Entities.VideoSurveillance
{
    [Table("vs_video_dev_equipment_info")]
    public class VideoDevEquipmentInfo : ISASFullAuditedEntity, IISASPassivable
    {
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 关联监控设备表
        /// </summary>
        public virtual Guid? EquipmentInfoId { get; set; }

        /// <summary>
        /// 关联视频设备表
        /// </summary>
        public virtual Guid? VideoDevId { get; set; }

        /// <summary>
        /// 所属场站
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [ForeignKey("EquipmentInfoId")]
        public EquipmentInfo EquipmentInfo { get; set; }

        [ForeignKey("VideoDevId")]
        public VideoDev VideoDev { get; set; }

        [ForeignKey("TransformerSubstationId")]
        public virtual TransformerSubstation TransformerSubstation { get; set; }
    }
}