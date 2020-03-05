using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using YunDa.ISAS.Entities.MongoDB;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionResultDto
{
    [AutoMapTo(typeof(InspectionResult))]
    public class EditInspectionResultInput : EntityDto<Guid?>
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
}