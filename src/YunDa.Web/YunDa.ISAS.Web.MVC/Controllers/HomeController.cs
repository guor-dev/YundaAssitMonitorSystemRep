using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using YunDa.ISAS.Application.Core.Session;

namespace ISAS.Web.MVC.Controllers
{
    public class HomeController : ISASControllerBase
    {
        private readonly ISessionAppService _sessionAppService;

        public HomeController(ISessionAppService sessionAppService)
        {
            _sessionAppService = sessionAppService;
        }

        public IActionResult Index()
        {
            var user = _sessionAppService.GetCurrentLoginInformations().User;
            ViewData["UserName"] = user.UserName;
            //ViewData["AccessToken"] = _configuration.CreateAccessToken(user);
            return View();
        }

        public IActionResult Home()
        {
            var user = _sessionAppService.GetCurrentLoginInformations().User;
            ViewData["UserName"] = user.UserName;
            return View();
        }

        public IActionResult Test()
        {
            var user = _sessionAppService.GetCurrentLoginInformations().User;
            ViewData["UserName"] = user.UserName;
            return View();
        }
    }
}