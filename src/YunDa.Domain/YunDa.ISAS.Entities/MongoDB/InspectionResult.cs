using Abp.Domain.Entities;
using System;
using System.ComponentModel;

namespace YunDa.ISAS.Entities.MongoDB
{
    /// <summary>
    /// 巡检结果
    /// </summary>
    public class InspectionResult : Entity<Guid>
    {
        /// <summary>
        /// 所属变电所
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        /// <summary>
        /// 巡检人员
        /// </summary>
        public virtual string InspectionPerson { get; set; }

        /// <summary>
        /// 巡检记录单ID，即：巡检线路ID
        /// </summary>
        public virtual Guid CardId { get; set; }

        /// <summary>
        /// 巡检记录单名称，即：巡检线路名称
        /// </summary>
        public virtual string CardName { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        public virtual DateTime InspectionTime { get; set; }

        /// <summary>
        /// 巡检类型 0:设备巡检；1:手动巡检
        /// </summary>
        public virtual InspectionResultType InspectionResultType { get; set; }
    }

    public enum InspectionResultType
    {
        [Description("设备巡检")]
        EquipmentInspection = 0,

        [Description("手动巡检")]
        ManualInspection = 1,
    }
}