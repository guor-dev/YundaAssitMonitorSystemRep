using Abp.Domain.Entities;
using System;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.Entities.AuditCommon
{
    public abstract class ISASAuditedEntity : Entity<Guid>, IISASAudited
    {
        public virtual Guid? CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
    }
}