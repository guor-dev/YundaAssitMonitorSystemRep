using System;

namespace YunDa.ISAS.Entities.AuditCommon.IAdudit
{
    public interface IDeletionAudited : ISoftDelete
    {
        Guid? DeleterUserId { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}