namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class DepositModalViewComponent : ViewComponent
    {
        private readonly IUserService userProfileService;
        private readonly UserManager<ApplicationUser> userManager;

        public DepositModalViewComponent(
            IUserService userProfileService,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.userProfileService = userProfileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync((ClaimsPrincipal)this.User);
            string verifiactionStatus = this.userProfileService.GetUserVerificationStatus(user);

            var depositViewModel = new DepositModalViewModel
            {
                IsVerified = verifiactionStatus == VerificationDocumentStatus.Verified.ToString(),
            };

            return this.View(depositViewModel);
        }
    }
}