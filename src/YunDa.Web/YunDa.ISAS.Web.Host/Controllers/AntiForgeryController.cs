using ISAS.Web.Core.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace YunDa.ISAS.Web.Host.Controllers
{
    public class AntiForgeryController : ISASControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}