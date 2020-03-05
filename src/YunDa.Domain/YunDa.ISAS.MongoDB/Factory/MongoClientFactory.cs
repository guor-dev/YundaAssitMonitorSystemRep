using MongoDB.Driver;
using System;
using YunDa.ISAS.MongoDB.Configuration;

namespace YunDa.ISAS.MongoDB.Factory
{
    public class MongoClientFactory : IMongoClientFactory
    {
        private readonly IMongoDBConfiguration _mongoDbConfiguration;
        private readonly object _locker = new object();
        private IMongoDatabase _singleDataBase;
        private MongoClient _singleMongoClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MongoClientFactory(IMongoDBConfiguration mongoDbConfiguration)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
        }

        [Obsolete]
        public IMongoDatabase InstanceMongoDatabase()
        {
            if (_singleDataBase == null)
            {
                lock (_locker)
                {
                    if (_singleDataBase == null)
                    {
                        _singleDataBase = InstanceMongoClient()
                            .GetDatabase(_mongoDbConfiguration.DatabaseName);
                    }
                }
            }
            return _singleDataBase;
        }

        public MongoClient InstanceMongoClient()
        {
            if (_singleMongoClient == null)
            {
                lock (_locker)
                {
                    if (_singleMongoClient == null)
                    {
                        MongoClientSettings settings = new MongoClientSettings();
                        settings.ConnectTimeout = new TimeSpan(0, 1, 0);
                        settings.MinConnectionPoolSize = 1;
                        settings.MaxConnectionPoolSize = 50;
                        settings.Server = new MongoServerAddress(_mongoDbConfiguration.HostString, _mongoDbConfiguration.Port);
                        if (_mongoDbConfiguration.IsAuth)
                            settings.Credential = MongoCredential.CreateCredential(_mongoDbConfiguration.DatabaseName, _mongoDbConfiguration.UserName, _mongoDbConfiguration.Password);
                        _singleMongoClient = new MongoClient(settings);
                    }
                }
            }
            return _singleMongoClient;
        }
    }
}