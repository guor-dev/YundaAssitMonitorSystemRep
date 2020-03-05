using Abp.Application.Services;
using YunDa.ISAS.DataTransferObject.Session;

namespace YunDa.ISAS.Application.Core.Session
{
    public interface ISessionAppService : IApplicationService
    {
        CurrentLoginInformationsOutput GetCurrentLoginInformations();
    }
}