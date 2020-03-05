using System;
using System.Collections.Generic;

namespace YunDa.ISAS.DataTransferObject.CommonDto
{
    public class TreeModelOutput
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string Ico { get; set; }
        public Guid? ParentId { get; set; }
        public List<TreeModelOutput> Children { get; set; }
    }
}