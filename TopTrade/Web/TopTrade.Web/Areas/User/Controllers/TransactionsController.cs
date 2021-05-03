namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;

    public class TransactionsController : BaseLoggedUserController
    {
        private readonly ITradeService tradeService;

        public TransactionsController(ITradeService tradeService)
        {
            this.tradeService = tradeService;
        }

        public IActionResult All(int id = 1)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            const int ItemsPerPage = 8;

            var viewModel = this.tradeService.GetTradeHistory(userId, id, ItemsPerPage);

            return this.View(viewModel);
        }
    }
}
