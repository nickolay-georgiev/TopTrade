namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;

    public class WithdrawFundsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserDashboardService userDashboardService;

        public WithdrawFundsViewComponent(
            UserManager<ApplicationUser> userManager,
            IUserDashboardService userDashboardService)
        {
            this.userManager = userManager;
            this.userManager = userManager;
            this.userDashboardService = userDashboardService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync((ClaimsPrincipal)this.User);

            var withdrawViewModel = this.userDashboardService.GetAvailableUserWithdrawData(user.Id);
            return this.View(withdrawViewModel);
        }
    }
}
