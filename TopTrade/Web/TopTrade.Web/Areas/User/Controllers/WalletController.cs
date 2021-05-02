namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.Controllers;

    public class WalletController : BaseLoggedUserController
    {
        private readonly IUserProfileService profileService;

        public WalletController(IUserProfileService profileService)
        {
            this.profileService = profileService;
        }

        public IActionResult All(int id = 1)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int itemPerPage = 1;

            var walletDataViewModel = this.profileService.GetWalletHitoryData(userId, id, itemPerPage);
            return this.View(walletDataViewModel);
        }
    }
}
