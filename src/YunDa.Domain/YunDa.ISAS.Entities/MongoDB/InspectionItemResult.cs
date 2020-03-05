using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace YunDa.ISAS.Entities.MongoDB
{
    /// <summary>
    /// 巡检项结果
    /// </summary>
    public class InspectionItemResult : Entity<Guid>
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
        /// 文件基本路径
        /// </summary>
        public virtual string BasePath { get; set; }

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public virtual string RelativePath { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public IEnumerable<InspectionItemFile> InspectionItemFiles { get; set; }

        #endregion 巡检项结果文件
    }

    /// <summary>
    /// 巡检项关联文件
    /// </summary>
    public class InspectionItemFile
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

    public enum FileType
    {
        [Description("照片")]
        Photo = 0,

        [Description("视频")]
        Video = 1
    }
}