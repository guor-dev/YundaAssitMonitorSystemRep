using Abp.Application.Services.Dto;
using System;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto
{
    public class VideoNVRProperty : EntityDto<Guid>
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
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

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

        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual Guid? TransformerSubstationId { get; set; }
    }
}