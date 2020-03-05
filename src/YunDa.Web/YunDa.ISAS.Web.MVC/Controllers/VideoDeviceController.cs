using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.MVC.Controllers
{
    public class VideoDeviceController : ISASControllerBase
    {
        [Authorize]
        public IActionResult VideoDevice()
        {
            return View();
        }
    }
}