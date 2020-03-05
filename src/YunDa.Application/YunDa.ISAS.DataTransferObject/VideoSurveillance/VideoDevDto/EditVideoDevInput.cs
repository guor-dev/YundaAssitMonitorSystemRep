using Abp.AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto
{
    [AutoMapTo(typeof(VideoDev))]
    public class EditVideoDevInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? seqNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Required]
        [StringLength(VideoDev.MaxNameLength)]
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
        [StringLength(VideoDev.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        #region 当设备是硬盘录像机类型的时候，一下字段为必填字段，否则为选填字段

        /// <summary>
        /// 设备IP
        /// </summary>
        [StringLength(VideoDev.MaxIpLength)]
        public virtual string IP { get; set; }

        /// <summary>
        /// 设备端口号
        /// </summary>
        public virtual int? Port { get; set; }

        /// <summary>
        /// 登录设备的用户名
        /// </summary>
        [StringLength(VideoDev.MaxDevUserNameLength)]
        public virtual string DevUserName { get; set; }

        /// <summary>
        /// 登录设备的密码
        /// </summary>
        [StringLength(VideoDev.MaxDevPasswordLength)]
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

        #endregion 仅当数据是摄像头类型的时候，需要设置的字段

        /// <summary>
        /// 是否启用
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        public virtual Guid? TransformerSubstationId { get; set; }
        public virtual Guid? VideoDevId { get; set; }
    }
}