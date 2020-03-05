using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.DataTransferObject.System.FunctionDto
{
    /// <summary>
    /// 系统功能输出文档
    /// </summary>
    [AutoMapFrom(typeof(SysFunction))]
    public class FunctionOutput : EntityDto<Guid>
    {
        public virtual int SeqNo { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 功能类别
        /// </summary>
        public virtual FunctionType Type { get; set; }

        /// <summary>
        /// 功能加载链接
        /// </summary>
        public virtual string LoadUrl { get; set; }

        /// <summary>
        /// 功能显示图标
        /// </summary>
        public virtual string Icon { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 自关联
        /// </summary>
        public virtual Guid? SysFunctionId { get; set; }
    }
}