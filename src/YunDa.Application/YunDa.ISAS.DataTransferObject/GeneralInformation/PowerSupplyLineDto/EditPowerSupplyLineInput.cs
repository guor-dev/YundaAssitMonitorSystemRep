using Abp.AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.GeneralInformation;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.PowerSupplyLineDto
{
    [AutoMapTo(typeof(PowerSupplyLine))]
    public class EditPowerSupplyLineInput : ISASAuditedEntityDto
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        [Required]
        [StringLength(InspectionPlanTask.MaxNameLength)]
        public string LineName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(InspectionPlanTask.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }
    }
}