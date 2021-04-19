namespace TopTrade.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data;
    using TopTrade.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {

        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            //var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            //return this.View(viewModel);
            return this.View();
        }
    }
}
