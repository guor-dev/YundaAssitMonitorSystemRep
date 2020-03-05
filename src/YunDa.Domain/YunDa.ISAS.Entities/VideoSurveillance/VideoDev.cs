using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Entities.VideoSurveillance
{
    /// <summary>
    /// 辅助视频设备（如NVR或DVR）
    /// </summary>
    [Table("vs_video_dev")]
    public class VideoDev : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxIpLength = 20;
        public const int MaxDevUserNameLength = 30;
        public const int MaxDevPasswordLength = 30;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? SeqNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string DevName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public virtual VideoDevTypeEnum DevType { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        public virtual Guid? ManufacturerInfoId { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        public virtual DateTime? InstallationDate { get; set; }

        /// <summary>
        /// 出厂时间
        /// </summary>
        public virtual DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        #region 当设备是硬盘录像机类型的时候，一下字段为必填字段，否则为选填字段

        /// <summary>
        /// 设备IP
        /// </summary>
        [StringLength(MaxIpLength)]
        public virtual string IP { get; set; }

        /// <summary>
        /// 设备端口号
        /// </summary>
        public virtual int? Port { get; set; }

        /// <summary>
        /// 登录设备的用户名
        /// </summary>
        [StringLength(MaxDevUserNameLength)]
        public virtual string DevUserName { get; set; }

        /// <summary>
        /// 登录设备的密码
        /// </summary>
        [StringLength(MaxDevPasswordLength)]
        public virtual string DevPassword { get; set; }

        #endregion 当设备是硬盘录像机类型的时候，一下字段为必填字段，否则为选填字段

        #region 仅当数据是摄像头类型的时候，需要设置的字段

        /// <summary>
        /// 摄像头占用的硬盘录像机通道号
        /// </summary>
        public virtual int? ChannelNo { get; set; }

        /// <summary>
        /// 设备通道号
        /// </summary>
        public virtual int? DevNo { get; set; }

        /// <summary>
        /// 是否支持云台
        /// </summary>
        [DefaultValue(false)]
        public virtual bool IsPTZ { get; set; }

        /// <summary>
        /// 默认播放码流
        /// </summary>
        public virtual CodeStreamTypeEnum CodeStreamType { get; set; }

        public virtual IEnumerable<PresetPoint> PresetPoints { get; set; }
        public virtual IEnumerable<InspectionItem> InspectionItems { get; set; }

        #endregion 仅当数据是摄像头类型的时候，需要设置的字段

        /// <summary>
        /// 是否启用
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        public virtual Guid? TransformerSubstationId { get; set; }
        public virtual Guid? VideoDevId { get; set; }

        [ForeignKey("TransformerSubstationId")]
        public virtual TransformerSubstation TransformerSubstation { get; set; }

        [ForeignKey("ManufacturerInfoId")]
        public virtual ManufacturerInfo ManufacturerInfo { get; set; }
    }

    public enum VideoDevTypeEnum
    {
        [Description("硬盘录像机")]
        硬盘录像机 = 0,

        [Description("枪机")]
        枪机 = 1,

        [Description("球机")]
        球机 = 2,

        [Description("热成像")]
        热成像 = 3,

        [Description("其他")]
        其他 = 99
    }

    public enum CodeStreamTypeEnum
    {
        [Description("主码流")]
        MainCodeStream = 1,

        [Description("子码流")]
        SubCodeStream = 2
    }
}