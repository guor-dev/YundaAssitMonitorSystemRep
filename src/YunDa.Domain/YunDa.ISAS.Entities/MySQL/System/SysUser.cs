using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.System
{
    [Table("sys_user")]
    public class SysUser : ISASAuditedEntity, IISASPassivable
    {
        /// <summary>
        /// Maximum length of the <see cref="UserName"/> property.
        /// </summary>
        public const int MaxNameLength = 50;

        /// <summary>
        /// Maximum length of the <see cref="UserName"/> property.
        /// </summary>
        public const int MaxPasswordLength = 50;

        /// <summary>
        /// Maximum length of the <see cref="PhoneNumber"/> property.
        /// </summary>
        public const int MaxPhoneNumberLength = 29;

        /// <summary>
        /// Maximum length of the <see cref="EmailAddress"/> property.
        /// </summary>
        public const int MaxEmailAddressLength = 50;

        /// <summary>
        /// UserName of the admin.
        /// admin can not be deleted and UserName of the admin can not be changed.
        /// </summary>
        public const string AdminUserName = "admin";

        public const string DefaultPassword = "123qwe";

        [Required]
        [StringLength(MaxNameLength)]
        public virtual string UserName { get; set; }

        [Required]
        [StringLength(MaxPasswordLength)]
        public virtual string Password { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string RealName { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [StringLength(MaxPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        [StringLength(MaxEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        ///// <summary>
        ///// 所属场站
        ///// </summary>
        //public virtual Guid? TransformerSubstationId { get; set; }
        ///// <summary>
        ///// 所属线路
        ///// </summary>
        //public virtual Guid? PowerSupplyLineId { get; set; }
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        public static SysUser CreateTenantAdminUser(string emailAddress, string userName = "")
        {
            var user = new SysUser
            {
                Id = Guid.NewGuid(),
                UserName = string.IsNullOrWhiteSpace(userName) ? AdminUserName : userName,
                RealName = string.IsNullOrWhiteSpace(userName) ? AdminUserName : userName,
                EmailAddress = emailAddress,
                PhoneNumber = "",
                IsActive = true
            };
            return user;
        }
    }
}