namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;

    public class WalletController : BaseLoggedUserController
    {
        private const int ItemsPerPage = 8;
        private readonly IUserService profileService;

        public WalletController(IUserService profileService)
        {
            this.profileService = profileService;
        }

        public IActionResult All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var walletDataViewModel = this.profileService.GetWalletHitoryData(userId, id, ItemsPerPage);
            return this.View(walletDataViewModel);
        }
    }
}
