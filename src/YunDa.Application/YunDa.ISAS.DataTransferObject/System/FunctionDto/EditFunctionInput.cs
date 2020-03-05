using Abp.AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.DataTransferObject.System.FunctionDto
{
    [AutoMapTo(typeof(SysFunction))]
    public class EditFunctionInput : ISASAuditedEntityDto
    {
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Required]
        [StringLength(SysFunction.MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        [Required]
        [StringLength(SysFunction.MaxNameLength)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 功能类别
        /// </summary>
        [Required]
        public virtual FunctionType Type { get; set; }

        /// <summary>
        /// 功能加载链接
        /// </summary>
        [StringLength(SysFunction.MaxUrlLength)]
        public virtual string LoadUrl { get; set; }

        /// <summary>
        /// 功能显示图标
        /// </summary>
        [StringLength(SysFunction.MaxIconLength)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(SysFunction.MaxRemarkLength)]
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
    }
}