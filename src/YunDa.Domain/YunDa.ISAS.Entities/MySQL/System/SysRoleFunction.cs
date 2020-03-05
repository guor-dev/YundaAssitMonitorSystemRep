using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.System
{
    [Table("sys_role_function")]
    public class SysRoleFunction : ISASAuditedEntity, IISASPassivable
    {
        /// <summary>
        /// 关联功能表
        /// </summary>
        public virtual Guid? SysFunctionId { get; set; }

        /// <summary>
        /// 关联权限表
        /// </summary>
        public virtual Guid? SysRoleId { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsEdit { get; set; }

        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        [ForeignKey("SysFunctionId")]
        public SysFunction SysFunction { get; set; }

        [ForeignKey("SysRoleId")]
        public SysRole SysRole { get; set; }
    }
}