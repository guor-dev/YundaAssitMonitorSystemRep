using System;

namespace YunDa.ISAS.Entities.AuditCommon.IAdudit
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}