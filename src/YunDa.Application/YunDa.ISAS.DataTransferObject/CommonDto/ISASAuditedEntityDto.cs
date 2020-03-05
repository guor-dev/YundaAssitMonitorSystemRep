using Abp.Application.Services.Dto;
using System;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.CommonDto
{
    public abstract class ISASAuditedEntityDto : EntityDto<Guid?>, IISASAudited
    {
        public virtual Guid? CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
    }
}