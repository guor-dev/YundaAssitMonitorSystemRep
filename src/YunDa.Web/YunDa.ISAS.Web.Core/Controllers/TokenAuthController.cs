using Abp.Authorization;
using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Authorization.Accounts;
using YunDa.ISAS.Core.Helper;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.Web.Core.Authentication;
using YunDa.ISAS.Web.Core.Models;

namespace YunDa.ISAS.Web.Core.Controllers
{
    [AbpAllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : ISASControllerBase
    {
        private readonly TokenAuthConfiguration _configuration;
        private readonly IAccountAppService _accountAppService;

        public TokenAuthController(IAccountAppService accountAppService, TokenAuthConfiguration configuration)
        {
            _accountAppService = accountAppService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            if (model == null) return null;
            var loginResult = await GetLoginResultAsync(model.UserName, model.Password).ConfigureAwait(false);
            if (loginResult.Flag)
            {
                AbpSession.Use(null, StringHelper.GuidToLongID(loginResult.ResultData.Id));
                var accessToken = _configuration.CreateAccessToken(loginResult.ResultData);
                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = _configuration.GetEncryptedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = loginResult.ResultData.Id
                };
            }
            else
                throw new Exception(loginResult.Message);
        }

        private async Task<RequestResult<LoginUserOutput>> GetLoginResultAsync(string usernameOrEmailAddress, string password)
        {
            return await _accountAppService.LoginAsync(usernameOrEmailAddress, password).ConfigureAwait(false);
        }
    }
}