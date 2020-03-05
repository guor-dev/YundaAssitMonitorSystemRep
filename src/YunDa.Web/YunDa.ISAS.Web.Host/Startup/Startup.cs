using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YunDa.ISAS.Web.Core.Configuration;

namespace YunDa.ISAS.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "cors";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddLogging();
            AuthConfigurer.Configure(services, _appConfiguration);
            services.AddSignalR();
            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.1.0",
                    Title = "ISAS WebAPI",
                    Description = "在线文档",
                });
                options.DocInclusionPredicate((docName, description) => true);
                //options.DocumentFilter<ShowApiFilter>();
                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                foreach (var item in XmlCommentsFilePath)
                {
                    options.IncludeXmlComments(item);
                }
            });
            // Configure Abp and Dependency Injection
            return services.AddAbp<ISASWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        private static List<string> XmlCommentsFilePath
        {
            get
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                DirectoryInfo d = new DirectoryInfo(basePath);
                //var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                FileInfo[] files = d.GetFiles("*.xml").Where(
                    f => f.Name == "YunDa.ISAS.Application.xml"
                || f.Name == "YunDa.ISAS.DataTransferObject.xml"
                || f.Name == "YunDa.ISAS.MongoDB.Application.xml").ToArray();
                var xmls = files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
                return xmls;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.
            app.UseCors(_defaultCorsPolicyName); // Enable CORS!
            app.UseStaticFiles();
            app.UseAuthorization().UseAuthentication();
            app.UseAbpRequestLocalization();
            app.UseRouting().UseEndpoints(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //区域路由
                routes.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            #region Swagger

            // Enable middleware to serve generated Swagger as a JSON endpoint
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "ISAS API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("YunDa.ISAS.Web.Host.wwwroot.swagger.ui.index.html");
                //options.RoutePrefix = string.Empty;
                options.DocExpansion(DocExpansion.None);//DocExpansion设置为none可折叠所有方法
                options.DefaultModelsExpandDepth(-1);//DefaultModelsExpandDepth设置为-1 可不显示models
            }); // URL: /swagger

            #endregion Swagger
        }
    }
}