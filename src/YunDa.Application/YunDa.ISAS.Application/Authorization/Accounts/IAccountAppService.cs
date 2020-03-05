using Abp.Application.Services;
using System.Threading.Tasks;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.System.UserDto;

namespace YunDa.ISAS.Application.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<RequestResult<LoginUserOutput>> LoginAsync(string userNameOrEmailAddress, string plainPassword);

        Task<RequestEasyResult> ChangePasswordAsync(ChangePasswordInput input);
    }
}