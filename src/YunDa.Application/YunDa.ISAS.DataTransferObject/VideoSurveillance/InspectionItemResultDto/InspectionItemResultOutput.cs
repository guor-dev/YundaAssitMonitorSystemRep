using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using YunDa.ISAS.Entities.MongoDB;

namespace YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemResultDto
{
    /// <summary>
    /// 巡检项结果
    /// </summary>
    [AutoMapFrom(typeof(InspectionItemResult))]
    public class InspectionItemResultOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 所属变电所
        /// </summary>
        public virtual Guid? TransformerSubstationId { get; set; }

        /// <summary>
        /// 巡检结果ID
        /// </summary>
        public virtual Guid InspectionResultId { get; set; }

        /// <summary>
        /// 巡检项ID，即：巡视点ID
        /// </summary>
        public virtual Guid ItemId { get; set; }

        /// <summary>
        /// 巡检项名称，即：巡视点名称
        /// </summary>
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 摄像头名称
        /// </summary>
        public virtual string CameraName { get; set; }

        /// <summary>
        /// 分析时间
        /// </summary>
        public virtual DateTime AnalysisTime { get; set; }

        /// <summary>
        /// 分析结果
        /// </summary>
        public virtual string AnalysisResult { get; set; }

        #region 巡检项结果文件

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public virtual string RelativePath { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public IEnumerable<InspectionItemFileOutput> InspectionItemFiles { get; set; }

        #endregion 巡检项结果文件
    }

    [AutoMapFrom(typeof(InspectionItemFile))]
    public class InspectionItemFileOutput
    {
        /// <summary>
        /// 文件类型 0：照片；1：视频
        /// </summary>
        public virtual FileType FileType { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public virtual string FileExtension { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public virtual string FileName { get; set; }
    }
}