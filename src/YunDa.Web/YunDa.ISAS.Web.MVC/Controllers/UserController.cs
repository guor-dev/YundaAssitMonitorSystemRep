using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.MVC.Controllers
{
    public class UserController : ISASControllerBase
    {
        public IActionResult UserPage()
        {
            return View();
        }
    }
}