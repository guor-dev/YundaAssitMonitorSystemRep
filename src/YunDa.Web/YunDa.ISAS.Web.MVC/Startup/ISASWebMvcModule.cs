using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using YunDa.ISAS.Web.Core;
using YunDa.ISAS.Web.Core.Configuration;

namespace YunDa.ISAS.Web.MVC.Startup
{
    [DependsOn(typeof(ISASWebCoreModule))]
    public class ISASWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ISASWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<ISASNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ISASWebMvcModule).GetAssembly());
        }
    }
}