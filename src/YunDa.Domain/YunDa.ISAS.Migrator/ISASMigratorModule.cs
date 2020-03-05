using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using YunDa.ISAS.Core;
using YunDa.ISAS.Core.Configuration;
using YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore;
using YunDa.ISAS.Migrator.DependencyInjection;

namespace YunDa.ISAS.Migrator
{
    [DependsOn(typeof(ISASEntityFrameworkCoreModule))]
    public class ISASMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ISASMigratorModule(ISASEntityFrameworkCoreModule isasEntityFrameworkModule)
        {
            isasEntityFrameworkModule.SkipDbSeed = true;
            _appConfiguration = ISASConfiguration.Get(typeof(ISASMigratorModule).GetAssembly().GetDirectoryPathOrNull());
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ISASConsts.ConnectionStringKey
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus),
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ISASMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}