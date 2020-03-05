using System;
using System.Collections.Generic;

namespace YunDa.ISAS.DataTransferObject.Session
{
    public class ApplicationInfoOutput
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Dictionary<string, bool> Features { get; set; }
    }
}