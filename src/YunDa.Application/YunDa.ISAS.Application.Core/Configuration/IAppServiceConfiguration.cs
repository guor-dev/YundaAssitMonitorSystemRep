using System;
using System.Collections.Generic;
using System.Text;

namespace YunDa.ISAS.Application.Core.Configuration
{
    public interface IAppServiceConfiguration
    {
        /// <summary>
        /// 系统附件文件夹
        /// </summary>
        string SysAttachmentFolder { get; set; }
    }
}