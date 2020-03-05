using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.MVC.Controllers
{
    public class VideoDevEquipmentInfoController : ISASControllerBase
    {
        [Authorize]
        public IActionResult VideoDevEquipmentInfo()
        {
            return View();
        }
    }
}