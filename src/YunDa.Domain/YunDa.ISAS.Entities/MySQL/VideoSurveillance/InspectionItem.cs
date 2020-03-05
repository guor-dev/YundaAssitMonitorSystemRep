using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.VideoSurveillance
{
    /// <summary>
    /// 视频巡检任务项
    /// </summary>
    [Table("vs_inspection_Item")]
    public class InspectionItem : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 任务项名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string ItemName { get; set; }

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

        #region 策略

        /// <summary>
        /// 巡检过程中执行的活动
        /// </summary>
        public virtual ProcessActionEnum ProcessAction { get; set; }

        /// <summary>
        /// 该过程开始执行到下一过程开始执行，所持续的时间，单位为秒
        /// </summary>
        public virtual int ProcessDuration { get; set; }

        /// <summary>
        /// 是否图像识别
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsImageRecognition { get; set; }

        #endregion 策略

        /// <summary>
        /// 所属任务单
        /// </summary>
        public virtual Guid? InspectionCardId { get; set; }

        /// <summary>
        /// 所属摄像头
        /// </summary>
        public virtual Guid? VideoDevId { get; set; }

        /// <summary>
        /// 所属预置点
        /// </summary>
        public virtual Guid? PresetPointId { get; set; }

        [ForeignKey("InspectionCardId")]
        public virtual InspectionCard InspectionCard { get; set; }

        [ForeignKey("VideoDevId")]
        public virtual VideoDev VideoDev { get; set; }

        [ForeignKey("PresetPointId")]
        public virtual PresetPoint PresetPoint { get; set; }
    }

    public enum ProcessActionEnum
    {
        /// <summary>
        /// 无其他活动
        /// </summary>
        [Description("无活动")]
        None = 0,

        /// <summary>
        /// 拍照
        /// </summary>
        [Description("拍照")]
        Photograph = 1,
    }
}