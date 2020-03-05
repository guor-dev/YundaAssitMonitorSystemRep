using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using System;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.Core.Helper;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.System.UserDto;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.Application.Authorization.Accounts
{
    /// <summary>
    /// 系统账户管理服务
    /// </summary>
    [AbpAllowAnonymous]
    public class AccountAppService : ISASAppServiceBase, IAccountAppService
    {
        private readonly IRepository<SysUser, Guid> _sysUserRepository;

        public AccountAppService(IRepository<SysUser, Guid> sysUserRepository, ISessionAppService sessionAppService) :
            base(sessionAppService)
        {
            _sysUserRepository = sysUserRepository;
        }

        /// <summary>
        /// 系统用户登录
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<RequestResult<LoginUserOutput>> LoginAsync(string userName, string password)
        {
            RequestResult<LoginUserOutput> loginRst = new RequestResult<LoginUserOutput>();
            if (userName.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                loginRst.Flag = false;
                loginRst.Message = ResultMsgConst.PasswordIsNull;
                return loginRst;
            }
            password = StringHelper.MD5Encrypt64(password);
            var sysUser = await _sysUserRepository.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password).ConfigureAwait(false);
            if (sysUser == null)
            {
                loginRst.Flag = false;
                loginRst.Message = ResultMsgConst.UserNameOrPasswordIsError;
                return loginRst;
            }
            else
            {
                loginRst.Flag = true;
                loginRst.Message = ResultMsgConst.LoginSuccess;
                loginRst.ResultData = ObjectMapper.Map<LoginUserOutput>(sysUser);
                return loginRst;
            }
        }

        public async Task<RequestEasyResult> ChangePasswordAsync(ChangePasswordInput input)
        {
            RequestEasyResult rst = new RequestEasyResult();
            if (input == null) return rst;
            if (input.CurrentPassword.IsNullOrEmpty() || input.NewPassword.IsNullOrEmpty())
            {
                rst.Flag = false;
                rst.Message = ResultMsgConst.OldAndNewPasswordIsNull;
                return rst;
            }
            var user = GetCurrentUser();
            if (user != null)
            {
                rst.Flag = false;
                rst.Message = ResultMsgConst.CurrentUserPasswordIsError;
                return rst;
            }
            var sysUser = await _sysUserRepository.FirstOrDefaultAsync(u => u.Id == user.Id).ConfigureAwait(false);
            sysUser.Password = StringHelper.MD5Encrypt64(input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            rst.Flag = true;
            return rst;
        }
    }
}