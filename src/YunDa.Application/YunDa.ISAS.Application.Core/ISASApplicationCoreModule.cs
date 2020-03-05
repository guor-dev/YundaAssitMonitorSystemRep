using Abp.AspNetCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using YunDa.ISAS.Application.Core.Configuration;
using YunDa.ISAS.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore;

namespace YunDa.ISAS.Application.Core
{
    [DependsOn(
        typeof(ISASCoreModule),
        typeof(ISASEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreModule),
        typeof(ISASDataTransferObjectModule))]
    public class ISASApplicationCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            ///替换默认审计服务实现
            //Configuration.ReplaceService(typeof(IAuditingStore), () =>
            //{
            //    IocManager.Register<IAuditingStore, ISASAuditingStore>(DependencyLifeStyle.Transient);
            //});
            Configuration.Auditing.IsEnabled = false;
            IocManager.Register<IAppServiceConfiguration, AppServiceConfiguration>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ISASApplicationCoreModule).GetAssembly();
            IocManager.RegisterAssemblyByConvention(thisAssembly);
        }
    }
}