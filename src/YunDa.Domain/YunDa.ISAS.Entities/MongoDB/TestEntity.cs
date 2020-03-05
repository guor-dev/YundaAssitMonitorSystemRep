using Abp.Domain.Entities;
using System;

namespace YunDa.ISAS.Entities.MongoDB
{
    public class TestEntity : Entity<Guid>
    {
        public string Name { get; set; }

        public DateTime DTime { get; set; }
    }
}