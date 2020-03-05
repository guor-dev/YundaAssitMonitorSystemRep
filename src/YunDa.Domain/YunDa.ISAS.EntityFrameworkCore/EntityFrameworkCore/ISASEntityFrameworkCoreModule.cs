using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using YunDa.ISAS.Core;
using YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore.Seed;

namespace YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore
{
    [DependsOn(
         typeof(ISASCoreModule),
         typeof(AbpEntityFrameworkCoreModule))]
    public class ISASEntityFrameworkCoreModule : AbpModule
    {
        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpEfCore().AddDbContext<ISASDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ISASEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}