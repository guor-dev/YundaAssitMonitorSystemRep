using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using YunDa.ISAS.Application;
using YunDa.ISAS.Application.Core.Configuration;
using YunDa.ISAS.Configuration;
using YunDa.ISAS.Core;
using YunDa.ISAS.MongoDB.Application;
using YunDa.ISAS.MongoDB.Configuration;
using YunDa.ISAS.Web.Core.Authentication;
using YunDa.ISAS.Web.Core.Configuration;

namespace YunDa.ISAS.Web.Core
{
    [DependsOn(
         typeof(ISASApplicationModule),
         typeof(ISASMongoDBApplicationModule),
         typeof(AbpAspNetCoreModule),
         typeof(AbpAspNetCoreSignalRModule)
     )]
    public class ISASWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ISASWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ISASConsts.ConnectionStringKey
            );

            Console.WriteLine("数据库连接：" + _appConfiguration.GetConnectionString(ISASConsts.ConnectionStringKey));

            #region 设置MogngoDBConfig

            var mongoConfiguration = Configuration.Modules.ISASConfiguration<IMongoDBConfiguration>();
            ConfigurationHelper.SetMongoDBConfiguration(_appConfiguration, ref mongoConfiguration);

            #endregion 设置MogngoDBConfig

            #region 设置AppConfig

            var appServiceConfiguration = Configuration.Modules.ISASConfiguration<IAppServiceConfiguration>();
            ConfigurationHelper.SetAppServiceConfiguration(_appConfiguration, ref appServiceConfiguration);

            #endregion 设置AppConfig

            // Use database for language management
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(ISASApplicationModule).GetAssembly()
                 );
            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();
            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ISASWebCoreModule).GetAssembly());
        }
    }
}