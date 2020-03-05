using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.DataTransferObject.System.UserDto
{
    [AutoMapFrom(typeof(SysUser))]
    public class UserOutput : EntityDto<Guid>
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string RealName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool IsActive { get; set; }
    }
}