using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.Controllers
{
    public class ManufacturerInfoController : ISASControllerBase
    {
        public IActionResult ManufacturerInfo()
        {
            return View();
        }
    }
}