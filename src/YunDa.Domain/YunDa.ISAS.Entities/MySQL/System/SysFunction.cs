using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YunDa.ISAS.Entities.AuditCommon;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.System
{
    [Table("sys_function")]
    public class SysFunction : ISASAuditedEntity, IISASPassivable
    {
        public const int MaxNameLength = 50;
        public const int MaxRemarkLength = 200;
        public const int MaxIconLength = 40;
        public const int MaxUrlLength = 200;
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 功能类别
        /// </summary>
        [Required]
        public virtual FunctionType Type { get; set; }

        /// <summary>
        /// 功能加载链接
        /// </summary>
        [StringLength(MaxUrlLength)]
        public virtual string LoadUrl { get; set; }

        /// <summary>
        /// 功能显示图标
        /// </summary>
        [StringLength(MaxIconLength)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 自关联
        /// </summary>
        public virtual Guid? SysFunctionId { get; set; }

        //    /// <summary>
        //    /// 功能子集合
        //    /// </summary>
        //    public virtual IEnumerable<SysFunction> SysFunctions { get; set; }
    }

    public enum FunctionType
    {
        /// <summary>
        /// Browser
        /// </summary>
        [Description("浏览器端")]
        Web = 0,

        /// <summary>
        /// Client
        /// </summary>
        [Description("客户端")]
        Client = 1,
    }
}