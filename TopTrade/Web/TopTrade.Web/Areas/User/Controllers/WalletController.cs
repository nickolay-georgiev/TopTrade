namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;

    public class WalletController : BaseLoggedUserController
    {
        private readonly IUserService profileService;

        public WalletController(IUserService profileService)
        {
            this.profileService = profileService;
        }

        public IActionResult All(int id = 1)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int itemPerPage = 8;

            var walletDataViewModel = this.profileService.GetWalletHitoryData(userId, id, itemPerPage);
            return this.View(walletDataViewModel);
        }
    }
}
