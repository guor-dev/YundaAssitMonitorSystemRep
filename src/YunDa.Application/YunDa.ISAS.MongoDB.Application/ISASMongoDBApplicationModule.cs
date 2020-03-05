using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using YunDa.ISAS.Core;

namespace YunDa.ISAS.MongoDB.Application
{
    [DependsOn(
       typeof(ISASCoreModule),
       typeof(ISASMongoDBModule)
       )]
    public class ISASMongoDBApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Authorization.Providers.Add<AbpProjectNameAuthorizationProvider>();
            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(typeof(ISASMongoDBApplicationModule).Assembly, moduleName: "isas", useConventionalHttpVerbs: true);
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ISASMongoDBApplicationModule).GetAssembly();
            IocManager.RegisterAssemblyByConvention(thisAssembly);
        }
    }
}