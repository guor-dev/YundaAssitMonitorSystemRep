using System;

namespace YunDa.ISAS.DataTransferObject.System.FunctionDto
{
    public class FunctionSearchConditionInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 自关联
        /// </summary>
        public virtual Guid? SysFunctionId { get; set; }
    }
}