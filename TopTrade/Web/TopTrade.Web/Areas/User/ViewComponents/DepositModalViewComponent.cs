namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class DepositModalViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DepositModalViewComponent(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ApplicationUser user = await this.userManager.GetUserAsync((ClaimsPrincipal)this.User);
            bool isVerified =
                user.Documents.Any() &&
                user.Documents.All(x => x.VerificationStatus == VerificationDocumentStatus.Approved.ToString());

            DepositModalInputModel depositViewModel = new DepositModalInputModel { IsVerified = isVerified };
            return this.View(depositViewModel);
        }
    }
}