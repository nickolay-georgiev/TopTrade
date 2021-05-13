namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using TopTrade.Services.Data.AccountManager;

    public class DashboardController : BaseAccountManagerController
    {
        private readonly IAccountManagementService accountManagementService;

        public DashboardController(
            IAccountManagementService accountManagementService)
        {
            this.accountManagementService = accountManagementService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.accountManagementService.GetManagerDashboardDataAsync();
            return this.View(viewModel);
        }
    }
}
