using System;

namespace YunDa.ISAS.DataTransferObject.CommonDto
{
    public class SelectModelOutput
    {
        public object Key { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public Guid? ParentId { get; set; }
    }
}