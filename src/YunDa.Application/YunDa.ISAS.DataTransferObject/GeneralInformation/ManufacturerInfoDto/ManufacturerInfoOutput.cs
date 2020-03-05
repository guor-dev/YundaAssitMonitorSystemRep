using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto
{
    [AutoMapFrom(typeof(ManufacturerInfo))]
    public class ManufacturerInfoOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 厂商名称
        /// </summary>
        public virtual string ManufacturerName { get; set; }

        /// <summary>
        /// 厂商电话
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 厂商邮件地址
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// 厂商邮件地址
        /// </summary>
        public virtual string ManufacturerAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool IsActive { get; set; }
    }
}