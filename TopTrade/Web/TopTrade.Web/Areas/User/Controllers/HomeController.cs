namespace TopTrade.Web.Areas.User
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Common;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.Areas.User.Controllers;
    using TopTrade.Web.ViewModels.User.Profile;

    public class HomeController : BaseLoggedUserController
    {
        private readonly IUserService userService;
        private readonly IStockService stockService;
        private readonly ITradeService tradeService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IUserService userService,
            IStockService stockService,
            ITradeService tradeService,
            UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.tradeService = tradeService;
            this.stockService = stockService;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole(GlobalConstants.AccountManagerRoleName))
            {
                return this.RedirectToAction(nameof(AccountManager.Controllers.DashboardController.Index), "Dashboard", new { area = "AccountManager" });
            }

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.RedirectToAction(nameof(Administration.Controllers.DashboardController.Index), "Dashboard", new { area = "Administration" });
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userData = await this.stockService.GetUserWatchlistWithStatisticAsync(userId);
            return this.View(userData);
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawFunds(WithdrawInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.ModelState.IsValid)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = "All fields are required";
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                await this.userService.AcceptWithdrawRequestAsync(input, userId);
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData["Success"] = "Success!";
            this.TempData["Message"] = "Your withdraw request was sent";

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
