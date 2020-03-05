using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace YunDa.ISAS.DataTransferObject
{
    [DependsOn(
    typeof(AbpAutoMapperModule))]
    public class ISASDataTransferObjectModule : AbpModule
    {
        public override void Initialize()
        {
            var thisAssembly = typeof(ISASDataTransferObjectModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}