using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore;

namespace YunDa.ISAS.Application
{
    [DependsOn(
        typeof(ISASApplicationCoreModule),
        typeof(ISASEntityFrameworkCoreModule)
       )]
    public class ISASApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Authorization.Providers.Add<AbpProjectNameAuthorizationProvider>();
            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(typeof(ISASApplicationModule).Assembly, moduleName: "isas", useConventionalHttpVerbs: true);
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ISASApplicationModule).GetAssembly();
            IocManager.RegisterAssemblyByConvention(thisAssembly);
        }
    }
}