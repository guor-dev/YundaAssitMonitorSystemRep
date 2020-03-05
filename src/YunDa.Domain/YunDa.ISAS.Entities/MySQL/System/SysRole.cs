using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.System
{
    [Table("sys_role")]
    public class SysRole : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 50;
        public const int MaxRemarkLength = 200;
        public const string AdminRole = "超级管理员";

        [Required]
        [StringLength(MaxNameLength)]
        public virtual string RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }
    }
}