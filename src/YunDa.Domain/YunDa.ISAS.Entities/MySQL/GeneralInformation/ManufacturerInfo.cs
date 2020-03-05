using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.GeneralInformation
{
    [Table("gi_manufacturer_info")]
    public class ManufacturerInfo : ISASFullAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 100;
        public const int MaxPhoneNumberLength = 29;
        public const int MaxEmailAddressLength = 50;
        public const int MaxAddressLength = 200;
        public const int MaxRemarkLength = 200;

        /// <summary>
        /// 厂商名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string ManufacturerName { get; set; }

        /// <summary>
        /// 厂商电话
        /// </summary>
        [StringLength(MaxPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 厂商邮件地址
        /// </summary>
        [StringLength(MaxEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// 厂商邮件地址
        /// </summary>
        [StringLength(MaxAddressLength)]
        public virtual string ManufacturerAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }
    }
}