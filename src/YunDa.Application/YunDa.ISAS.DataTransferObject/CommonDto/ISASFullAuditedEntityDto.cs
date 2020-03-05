using Abp.Application.Services.Dto;
using System;
using YunDa.ISAS.Entities.AuditCommon.IAdudit;

namespace YunDa.ISAS.DataTransferObject.CommonDto
{
    public class ISASFullAuditedEntityDto : EntityDto<Guid?>, IFullAudited
    {
        public virtual Guid? CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual Guid? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}