using Abp.MultiTenancy;

namespace ISAS.Web.MVC.Models.Account
{
    public class LoginFormViewModel
    {
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }

        public bool IsSelfRegistrationAllowed { get; set; }

        public MultiTenancySides MultiTenancySide { get; set; }

        public string LoginMessage { get; set; }
    }
}