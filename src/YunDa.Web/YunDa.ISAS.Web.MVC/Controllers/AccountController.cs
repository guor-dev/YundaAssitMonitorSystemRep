using Abp.Extensions;
using ISAS.Web.Core.Controllers;
using ISAS.Web.MVC.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Authorization.Accounts;
using YunDa.ISAS.Core.Helper;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.Web.Core;
using YunDa.ISAS.Web.Core.Authentication;

namespace ISAS.Web.MVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : ISASControllerBase
    {
        private readonly IAccountAppService _accountAppService;

        public AccountController(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        #region Login / Logout

        public ActionResult Login(string userName = "", string returnUrl = "", string successMessage = "")
        {
            //if (string.IsNullOrWhiteSpace(returnUrl))
            //{
            //    returnUrl = GetAppHomeUrl();
            //}
            returnUrl = GetAppHomeUrl();
            return View(new LoginFormViewModel
            {
                UserName = userName,
                ReturnUrl = returnUrl,
                IsSelfRegistrationAllowed = false,
                MultiTenancySide = AbpSession.MultiTenancySide,
                LoginMessage = successMessage
            });
        }

        [HttpPost]
        public virtual async Task<ActionResult> LoginAction(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            var loginResult = await GetLoginResultAsync(loginModel.UserName, loginModel.Password);
            if (loginResult.Flag)
            {
                //用户标识
                var identity = new ClaimsIdentity(ISASWebCoreConst.AuthenticationType);
                identity.AddClaims(TokenAuthConfiguration.CreateClaims(loginResult.ResultData));
                HttpContext.User = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(ISASWebCoreConst.AuthenticationType, new ClaimsPrincipal(identity),
                         new AuthenticationProperties
                         {
                             IsPersistent = loginModel.RememberMe == true,
                         });
                //var accessToken = _configuration.CreateAccessToken(loginResult.ResultData);
                return Redirect(returnUrl);
            }
            else
                return RedirectToAction("Login", "Account", new { userName = loginModel.UserName, returnUrl = returnUrl, successMessage = loginResult.Message });
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(ISASWebCoreConst.AuthenticationType);
            return RedirectToAction("Login");
        }

        private async Task<RequestResult<LoginUserOutput>> GetLoginResultAsync(string usernameOrEmailAddress, string password)
        {
            return await _accountAppService.LoginAsync(usernameOrEmailAddress, password);
        }

        #endregion Login / Logout

        #region Helpers

        public ActionResult RedirectToAppHome()
        {
            return RedirectToAction("Index", "Home");
        }

        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

        #endregion Helpers

        #region Common

        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = GetAppHomeUrl;
            }

            if (returnUrl.IsNullOrEmpty())
            {
                return defaultValueBuilder();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return defaultValueBuilder();
        }

        #endregion Common
    }
}