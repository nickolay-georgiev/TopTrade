namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;

    public class UserProfileCardViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserProfileCardViewComponent(
            UserManager<ApplicationUser> userManager,
            IUserService editProfileService)
        {
            this.userManager = userManager;
            this.userService = editProfileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var userProfileCardViewModel = this.userService.GetUserDataCardComponent(user);

            return this.View(userProfileCardViewModel);
        }
    }
}
