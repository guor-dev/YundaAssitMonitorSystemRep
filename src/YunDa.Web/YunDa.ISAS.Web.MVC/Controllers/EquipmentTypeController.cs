using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YunDa.ISAS.Web.Controllers
{
    public class EquipmentTypeController : ISASControllerBase
    {
        public IActionResult EquipmentType()
        {
            return View();
        }
    }
}