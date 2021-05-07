﻿namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;

    public class TransactionsController : BaseLoggedUserController
    {
        private readonly ITradeService tradeService;
        private readonly UserManager<ApplicationUser> userManager;

        public TransactionsController(
            ITradeService tradeService,
            UserManager<ApplicationUser> userManager)
        {
            this.tradeService = tradeService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int id = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            const int ItemsPerPage = 8;

            var viewModel = this.tradeService.GetTradeHistory(user, id, ItemsPerPage);

            return this.View(viewModel);
        }
    }
}
