using Microsoft.Extensions.Configuration;
using YunDa.ISAS.Application.Core.Configuration;
using YunDa.ISAS.MongoDB.Configuration;

namespace YunDa.ISAS.Configuration
{
    public static class ConfigurationHelper
    {
        public const string MongoDBHostKey = "MongoDBSetting:Host";
        public const string MongoDBPortKey = "MongoDBSetting:Port";
        public const string MongoDBDatabaseNameKey = "MongoDBSetting:DatabaseName";
        public const string MongoDBUserNameKey = "MongoDBSetting:UserName";
        public const string MongoDBPassWordKey = "MongoDBSetting:PassWord";
        public const string MongoDBIsAuthKey = "MongoDBSetting:IsAuth";

        public static void SetMongoDBConfiguration(IConfiguration appConfiguration, ref IMongoDBConfiguration mongoConfiguration)
        {
            int mongoDBPort = 27017;
            bool mongoDBIsAuth = false;
            int.TryParse(appConfiguration.GetConnectionString(MongoDBPortKey), out mongoDBPort);
            bool.TryParse(appConfiguration.GetConnectionString(MongoDBIsAuthKey), out mongoDBIsAuth);
            mongoConfiguration.HostString = appConfiguration.GetConnectionString(MongoDBHostKey);
            mongoConfiguration.Port = mongoDBPort;
            mongoConfiguration.DatabaseName = appConfiguration.GetConnectionString(MongoDBDatabaseNameKey);
            mongoConfiguration.UserName = appConfiguration.GetConnectionString(MongoDBUserNameKey);
            mongoConfiguration.Password = appConfiguration.GetConnectionString(MongoDBPassWordKey);
        }

        public const string SysAttachmentFolder = "SysBaseSetting:SysAttachmentFolder";

        public static void SetAppServiceConfiguration(IConfiguration appConfiguration, ref IAppServiceConfiguration appServiceConfiguration)
        {
            appServiceConfiguration.SysAttachmentFolder = appConfiguration[SysAttachmentFolder];
        }
    }
}