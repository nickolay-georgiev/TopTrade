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

        public IActionResult All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            const int ItemsPerPage = 8;

            var viewModel = this.tradeService.GetTradeHistory(userId, id, ItemsPerPage);

            if (id > viewModel.PagesCount)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}
