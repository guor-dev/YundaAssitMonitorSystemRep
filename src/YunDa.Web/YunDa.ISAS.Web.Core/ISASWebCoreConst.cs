using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace YunDa.ISAS.Web.Core
{
    public static class ISASWebCoreConst
    {
        public const string CookieType = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string AuthenticationType = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string AuthenticationTypeHost = "JwtBearer";
        public const string AuthenticationTypeJwtBearerDefaults = JwtBearerDefaults.AuthenticationScheme;
    }
}