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
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class DepositController : BaseLoggedUserController
    {
        private readonly IUserDashboardService userDashboardService;

        public DepositController(IUserDashboardService userDashboardService)
        {
            this.userDashboardService = userDashboardService;
        }

        [HttpPost]
        public async Task<IActionResult> MakeDeposit(DepositModalInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.userDashboardService.UpdateUserAccountAsync(input, userId);
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;

                return this.RedirectToAction("Index", "Home");
            }

            this.TempData["Success"] = "Success!";
            this.TempData["Message"] = "Your account balance was updated!";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
