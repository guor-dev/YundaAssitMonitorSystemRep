using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using YunDa.ISAS.Core.Configuration;

namespace YunDa.ISAS.Web.Core.Configuration
{
    public static class HostingEnvironmentExtensions
    {
        public static IConfigurationRoot GetAppConfiguration(this IWebHostEnvironment env)
        {
            if (env == null) return null;
            return ISASConfiguration.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }
    }
}