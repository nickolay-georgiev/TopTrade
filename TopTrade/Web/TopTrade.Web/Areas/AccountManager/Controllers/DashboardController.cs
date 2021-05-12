namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.AccountManager;

    public class DashboardController : BaseAccountManagerController
    {
        private readonly IAccountManagementService accountManagementService;

        public DashboardController(
            IAccountManagementService accountManagementService)
        {
            this.accountManagementService = accountManagementService;
        }

        public IActionResult Index()
        {
            var viewModel = this.accountManagementService.GetManagerDashboardData();
            return this.View(viewModel);
        }
    }
}
