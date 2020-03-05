using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.DataTransferObject.System.UserDto
{
    [AutoMapTo(typeof(SysUser))]
    public class EditUserInput : ISASAuditedEntityDto
    {
        [Required]
        [StringLength(SysUser.MaxNameLength)]
        public virtual string UserName { get; set; }

        [Required]
        [StringLength(SysUser.MaxPasswordLength)]
        public virtual string Password { get; set; }

        public virtual string RealName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool IsActive { get; set; }
    }
}