using Abp.Configuration.Startup;

namespace YunDa.ISAS.Configuration
{
    public static class ISASConfigurationExtensions
    {
        public static T ISASConfiguration<T>(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<T>();
        }
    }
}