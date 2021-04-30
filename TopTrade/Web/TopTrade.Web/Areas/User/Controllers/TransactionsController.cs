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

    public class TransactionsController : BaseLoggedUserController
    {
        private readonly ITradeService tradeService;

        public TransactionsController(ITradeService tradeService)
        {
            this.tradeService = tradeService;
        }

        public async Task<IActionResult> Index()
        {

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //if (id <= 0)
            //{
            //    return this.NotFound();
            //}

            const int ItemsPerPage = 8;

            var viewModel = this.tradeService.GetTradeHistory(userId, 1, ItemsPerPage);

            return this.View(viewModel);
        }
    }
}
