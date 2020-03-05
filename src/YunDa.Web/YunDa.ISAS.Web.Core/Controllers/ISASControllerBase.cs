using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Runtime.Session;
using YunDa.ISAS.Core;

namespace ISAS.Web.Core.Controllers
{
    [AbpMvcAuthorize]
    public abstract class ISASControllerBase : AbpController
    {
        protected ISASControllerBase()
        {
            LocalizationSourceName = ISASConsts.LocalizationSourceName;
            AbpSession = NullAbpSession.Instance;
        }
    }
}