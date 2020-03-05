using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.Web.Core.Models
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(SysUser.MaxNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(SysUser.MaxPasswordLength)]
        public string Password { get; set; }

        public bool RememberClient { get; set; }
    }
}