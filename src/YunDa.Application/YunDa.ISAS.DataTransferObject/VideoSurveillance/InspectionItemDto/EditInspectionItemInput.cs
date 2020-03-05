using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto
{
    [AutoMapTo(typeof(InspectionItem))]
    public class EditInspectionItemInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        public virtual int SeqNo { get; set; } = 0;

        /// <summary>
        /// 任务项名称
        /// </summary>
        [Required]
        [StringLength(InspectionItem.MaxNameLength)]
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(InspectionItem.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        #region 策略

        /// <summary>
        /// 巡检过程中执行的活动
        /// </summary>
        [Required]
        public virtual ProcessActionEnum ProcessAction { get; set; }

        /// <summary>
        /// 该过程开始执行到下一过程开始执行，所持续的时间，单位为秒
        /// </summary>
        public virtual int ProcessDuration { get; set; } = 0;

        /// <summary>
        /// 是否图像识别
        /// </summary>
        public virtual bool IsImageRecognition { get; set; } = true;

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
    }
}