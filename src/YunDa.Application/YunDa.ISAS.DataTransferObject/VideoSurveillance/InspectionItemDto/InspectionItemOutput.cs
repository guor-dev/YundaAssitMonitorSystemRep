using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto
{
    [AutoMapFrom(typeof(InspectionItem))]
    public class InspectionItemOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 任务项名称
        /// </summary>
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
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
        public virtual bool IsImageRecognition { get; set; }

        #endregion 策略

        /// <summary>
        /// 所属任务单
        /// </summary>
        public virtual Guid? InspectionCardId { get; set; }

        public virtual InspectionCardProperty InspectionCard { get; set; }

        /// 所属摄像头
        /// </summary>
        public virtual Guid? VideoDevId { get; set; }

        public virtual VideoDevProperty VideoDev { get; set; }

        /// <summary>
        /// 所属预置点
        /// </summary>
        public virtual Guid? PresetPointId { get; set; }

        public virtual PresetPointProperty PresetPoint { get; set; }
    }
}