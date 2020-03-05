using System;

namespace YunDa.ISAS.Entities.AuditCommon.IAdudit
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid? CreatorUserId { get; set; }
    }
}