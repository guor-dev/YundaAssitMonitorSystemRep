using MongoDB.Driver;

namespace YunDa.ISAS.MongoDB.Factory
{
    public interface IMongoClientFactory
    {
        IMongoDatabase InstanceMongoDatabase();
    }
}