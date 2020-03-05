using Abp.Reflection.Extensions;
using System;
using System.IO;

namespace YunDa.ISAS.Core
{
    /// <summary>
    /// Central point for application version.
    /// </summary>
    public static class AppVersionHelper
    {
        /// <summary>
        /// Gets current version of the application.
        /// It's also shown in the web page.
        /// </summary>
        public const string Version = "2.0.0.0";

        /// <summary>
        /// Gets release (last build) date of the application.
        /// It's shown in the web page.
        /// </summary>
        public static DateTime ReleaseDate
        {
            get { return new FileInfo(typeof(AppVersionHelper).GetAssembly().Location).LastWriteTime; }
        }

        public const string CompanyName = "交大运达电气";
        public const string CompanyAddress = "四川省成都市高新西区新达路11号";
    }
}