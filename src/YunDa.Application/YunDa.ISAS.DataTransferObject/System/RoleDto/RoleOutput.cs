using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.DataTransferObject.System.RoleDto
{
    [AutoMapFrom(typeof(SysRole))]
    public class RoleOutput : EntityDto<Guid>
    {
        [Required]
        [StringLength(SysRole.MaxNameLength)]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }
    }
}