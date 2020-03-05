using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.DataTransferObject.Session
{
    /// <summary>
    /// 登录用户返回Model
    /// </summary>
    [AutoMapFrom(typeof(SysUser))]
    public class LoginUserOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        public virtual string RealName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
    }
}