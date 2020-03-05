using Abp.Auditing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using YunDa.ISAS.Core;
using YunDa.ISAS.DataTransferObject.Session;

namespace YunDa.ISAS.Application.Core.Session

{
    public class SessionAppService : ISessionAppService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SessionAppService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [DisableAuditing]
        public CurrentLoginInformationsOutput GetCurrentLoginInformations()
        {
            var output = new CurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoOutput
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (_contextAccessor.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                string userDataJsonStr = _contextAccessor.HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.UserData).FirstOrDefault().Value;
                output.User = JsonConvert.DeserializeObject<LoginUserOutput>(userDataJsonStr);
            }
            return output;
        }
    }
}