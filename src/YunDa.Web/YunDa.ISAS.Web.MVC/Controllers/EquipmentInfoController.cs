using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.Controllers
{
    public class EquipmentInfoController : ISASControllerBase
    {
        public IActionResult EquipmentInfo()
        {
            return View();
        }
    }
}