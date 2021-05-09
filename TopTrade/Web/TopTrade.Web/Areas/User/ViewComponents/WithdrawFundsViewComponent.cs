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
        private readonly IUserService userService;

        public WithdrawFundsViewComponent(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync((ClaimsPrincipal)this.User);
            var withdrawViewModel = this.userService.GetAvailableUserWithdrawData(user.Id);
            return this.View(withdrawViewModel);
        }
    }
}
