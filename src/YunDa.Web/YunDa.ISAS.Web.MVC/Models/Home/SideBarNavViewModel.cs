using Abp.Application.Navigation;

namespace ISAS.Web.MVC.Models.Home
{
    public class SideBarNavViewModel
    {
        public UserMenu MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }
    }
}