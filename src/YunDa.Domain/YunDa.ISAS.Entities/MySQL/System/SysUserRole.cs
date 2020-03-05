using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.System
{
    [Table("sys_user_role")]
    public class SysUserRole : ISASAuditedEntity, IISASPassivable
    {
        /// <summary>
        /// 关联权限表
        /// </summary>
        public virtual Guid? SysUserId { get; set; }

        /// <summary>
        /// 关联权限表
        /// </summary>
        public virtual Guid? SysRoleId { get; set; }

        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        [ForeignKey("SysRoleId")]
        public SysUser SysUser { get; set; }

        [ForeignKey("SysRoleId")]
        public SysRole SysRole { get; set; }
    }
}