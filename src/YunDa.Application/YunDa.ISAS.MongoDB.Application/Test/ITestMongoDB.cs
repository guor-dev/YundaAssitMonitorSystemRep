using Abp.Application.Services;
using System.Threading.Tasks;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.Entities.MongoDB;

namespace YunDa.ISAS.Test
{
    public interface ITestMongoDB : IApplicationService
    {
        Task<RequestEasyResult> TestMongoDBAdd();

        RequestPageResult<TestEntity> TestMongoDBSearch();
    }
}