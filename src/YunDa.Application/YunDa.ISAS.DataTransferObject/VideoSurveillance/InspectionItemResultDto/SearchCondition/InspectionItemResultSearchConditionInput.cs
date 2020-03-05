using System;
using System.Collections.Generic;
using System.Text;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemResultDto.SearchCondition
{
    public class InspectionItemResultSearchConditionInput
    {
        /// <summary>
        /// 巡检结果ID
        /// </summary>
        public virtual Guid? InspectionResultId { get; set; }

        /// <summary>
        /// 巡检项名称
        /// </summary>
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 最大分析时间
        /// </summary>
        public virtual DateTime? MaxAnalysisTime { get; set; }

        /// <summary>
        /// 最小分析时间
        /// </summary>
        public virtual DateTime? MinAnalysisTime { get; set; }
    }
}