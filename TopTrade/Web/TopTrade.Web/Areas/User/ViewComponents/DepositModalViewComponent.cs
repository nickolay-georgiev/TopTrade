namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class DepositModalViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userProfileService;

        public DepositModalViewComponent(
            UserManager<ApplicationUser> userManager,
            IUserService userProfileService)
        {
            this.userManager = userManager;
            this.userProfileService = userProfileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync((ClaimsPrincipal)this.User);
            string verifiactionStatus = this.userProfileService.GetUserVerificationStatus(user);

            var depositViewModel = new DepositModalInputModel
            {
                IsVerified = verifiactionStatus == "Verified",
            };

            return this.View(depositViewModel);
        }
    }
}