using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto
{
    [AutoMapFrom(typeof(VideoDev))]
    public class VideoDevOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? SeqNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
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
        public virtual string Remark { get; set; }

        #region 当设备是硬盘录像机类型的时候，一下字段为必填字段，否则为选填字段

        /// <summary>
        /// 设备IP
        /// </summary>
        public virtual string IP { get; set; }

        /// <summary>
        /// 设备端口号
        /// </summary>
        public virtual int? Port { get; set; }

        /// <summary>
        /// 登录设备的用户名
        /// </summary>
        public virtual string DevUserName { get; set; }

        /// <summary>
        /// 登录设备的密码
        /// </summary>
        public virtual string DevPassword { get; set; }

        #endregion 当设备是硬盘录像机类型的时候，一下字段为必填字段，否则为选填字段

        #region 仅当数据是摄像头类型的时候，需要设置的字段

        /// <summary>
        /// 摄像头占用的通道号
        /// </summary>
        public virtual int? ChannelNo { get; set; }

        /// <summary>
        /// 设备通道号
        /// </summary>
        public virtual int? DevNo { get; set; }

        /// <summary>
        /// 是否支持云台
        /// </summary>
        public virtual bool IsPTZ { get; set; }

        /// <summary>
        /// 默认播放码流
        /// </summary>
        public virtual CodeStreamTypeEnum CodeStreamType { get; set; }

        #endregion 仅当数据是摄像头类型的时候，需要设置的字段

        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual Guid? TransformerSubstationId { get; set; }
        public virtual Guid? VideoDevId { get; set; }
        public virtual ManufacturerInfoProperty ManufacturerInfo { get; set; }
        public virtual IEnumerable<PresetPointProperty> PresetPoints { get; set; }
        public virtual IEnumerable<VideoDevOutput> Children { get; set; }
    }
}