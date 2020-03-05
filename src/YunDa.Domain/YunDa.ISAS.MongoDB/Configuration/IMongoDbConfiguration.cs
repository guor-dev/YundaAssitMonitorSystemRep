namespace YunDa.ISAS.MongoDB.Configuration
{
    public interface IMongoDBConfiguration
    {
        string HostString { get; set; }
        int Port { get; set; }

        string DatabaseName { get; set; }
        string UserName { get; set; }

        string Password { get; set; }
        bool IsAuth { get; set; }
    }
}