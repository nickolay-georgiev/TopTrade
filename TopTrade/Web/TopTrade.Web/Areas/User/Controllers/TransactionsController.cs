namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;

    public class TransactionsController : BaseLoggedUserController
    {
        private const int ItemsPerPage = 8;
        private readonly ITradeService tradeService;

        public TransactionsController(
            ITradeService tradeService,
            UserManager<ApplicationUser> userManager)
        {
            this.tradeService = tradeService;
        }

        public IActionResult All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewModel = this.tradeService.GetTradeHistory(userId, id, ItemsPerPage);

            return this.View(viewModel);
        }
    }
}
