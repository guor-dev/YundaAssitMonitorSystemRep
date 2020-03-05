using Abp.AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto
{
    [AutoMapTo(typeof(ManufacturerInfo))]
    public class EditManufacturerInfoInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 厂商名称
        /// </summary>
        [Required]
        [StringLength(ManufacturerInfo.MaxNameLength)]
        public virtual string ManufacturerName { get; set; }

        /// <summary>
        /// 厂商电话
        /// </summary>
        [StringLength(ManufacturerInfo.MaxPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 厂商邮件地址
        /// </summary>
        [StringLength(ManufacturerInfo.MaxEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// 厂商邮件地址
        /// </summary>
        [StringLength(ManufacturerInfo.MaxAddressLength)]
        public virtual string ManufacturerAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(ManufacturerInfo.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }
    }
}