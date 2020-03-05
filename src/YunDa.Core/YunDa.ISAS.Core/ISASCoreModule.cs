using Abp.Modules;
using Abp.Reflection.Extensions;

namespace YunDa.ISAS.Core
{
    public class ISASCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ISASCoreModule).GetAssembly());
        }
    }
}