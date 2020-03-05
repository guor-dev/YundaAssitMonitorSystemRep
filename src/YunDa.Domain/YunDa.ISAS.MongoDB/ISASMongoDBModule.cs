using Abp;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using YunDa.ISAS.Entities.MongoDB;
using YunDa.ISAS.MongoDB.Configuration;
using YunDa.ISAS.MongoDB.Factory;
using YunDa.ISAS.MongoDB.Repositories;

namespace YunDa.ISAS.MongoDB
{
    [DependsOn(typeof(AbpKernelModule))]
    public class ISASMongoDBModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IMongoDBConfiguration, MongoDBConfiguration>();
            IocManager.Register<IMongoClientFactory, MongoClientFactory>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ISASMongoDBModule).GetAssembly());
            this.RegisterRepository();
        }

        #region 注册仓储实体

        private void RegisterRepository()
        {
            IocManager.Register<IMongoDbRepository<TestEntity, Guid>, MongoDbRepository<TestEntity, Guid>>();
            IocManager.Register<IMongoDbRepository<InspectionResult, Guid>, MongoDbRepository<InspectionResult, Guid>>();
            IocManager.Register<IMongoDbRepository<InspectionItemResult, Guid>, MongoDbRepository<InspectionItemResult, Guid>>();
        }

        #endregion 注册仓储实体
    }
}