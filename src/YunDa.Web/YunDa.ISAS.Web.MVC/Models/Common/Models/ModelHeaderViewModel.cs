namespace ISAS.Web.MVC.Models.Common.Modals
{
    public class ModelHeaderViewModel
    {
        public string Title { get; set; }

        public ModelHeaderViewModel(string title)
        {
            Title = title;
        }
    }
}