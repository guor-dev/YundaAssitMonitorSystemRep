using Abp.AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.CommonDto;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto
{
    [AutoMapTo(typeof(EquipmentType))]
    public class EditEquipmentTypeInput : ISASAuditedEntityDto
    {
        public const int MaxNameLength = 50;

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(EquipmentType.MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsActive { get; set; }

        //public virtual bool IsSelected { get; set; }
    }
}