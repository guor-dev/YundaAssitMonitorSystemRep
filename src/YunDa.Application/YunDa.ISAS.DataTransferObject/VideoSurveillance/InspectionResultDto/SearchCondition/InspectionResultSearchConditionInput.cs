using System;
using System.Collections.Generic;
using System.Text;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionResultDto.SearchCondition
{
    /// <summary>
    /// 巡检结果查询条件
    /// </summary>
    public class InspectionResultSearchConditionInput
    {
        /// <summary>
        /// 巡检记录单名称，即：巡检线路名称
        /// </summary>
        public virtual string CardName { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        public virtual DateTime? InspectionTime { get; set; }
    }
}