using System;

namespace YunDa.ISAS.Entities.AuditCommon.IAdudit
{
    public interface IModificationAudited
    {
        DateTime? LastModificationTime { get; set; }
        Guid? LastModifierUserId { get; set; }
    }
}