namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.ViewModels.User.Stock;

    public class PortfolioController : BaseLoggedUserController
    {
        private readonly IStockService stockService;
        private readonly ITradeService tradeService;

        public PortfolioController(
            IStockService stockService,
            ITradeService tradeService)
        {
            this.stockService = stockService;
            this.tradeService = tradeService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var portfolioVewModel = await this.stockService.GetPortoflioAsync(userId);

            return this.View(portfolioVewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CloseTrade(int id, CloseTradeInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.tradeService.CloseTradeAsync(id, input, userId);
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;
                this.RedirectToAction(nameof(this.Index));
            }

            this.TempData["Success"] = "Success";
            this.TempData["Message"] = "Trade was closed";

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
