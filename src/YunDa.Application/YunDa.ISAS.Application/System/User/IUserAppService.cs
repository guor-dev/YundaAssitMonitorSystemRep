using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject.System.UserDto;

namespace YunDa.ISAS.Application.System
{
    public interface IUserAppService : IAppServiceBase<UserSearchConditionInput, UserOutput, EditUserInput, Guid>
    {
    }
}