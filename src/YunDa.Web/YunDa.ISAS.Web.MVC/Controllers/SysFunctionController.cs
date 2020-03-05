using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.MVC.Controllers
{
    [Authorize]
    public class SysFunctionController : ISASControllerBase
    {
        public IActionResult SysFunction()
        {
            return View();
        }
    }
}