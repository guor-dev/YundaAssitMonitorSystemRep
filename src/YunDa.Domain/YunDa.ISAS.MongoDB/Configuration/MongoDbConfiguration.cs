namespace YunDa.ISAS.MongoDB.Configuration
{
    public class MongoDBConfiguration : IMongoDBConfiguration
    {
        public string DatabaseName { get; set; }
        public string HostString { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public bool IsAuth { get; set; }
    }
}