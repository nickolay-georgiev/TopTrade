namespace TopTrade.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using TopTrade.Services.Data.Administrator;

    public class DashboardController : BaseAdministrationController
    {
        private readonly IAdministrationService administrationService;

        public DashboardController(
            IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.administrationService.GetAdminDashboardDataAsync();
            return this.View(viewModel);
        }
    }
}
