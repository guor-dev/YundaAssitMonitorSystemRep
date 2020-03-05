using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.MVC.Controllers
{
    public class VideoInspectionController : ISASControllerBase
    {
        public IActionResult VideoInspection()
        {
            return View();
        }
    }
}