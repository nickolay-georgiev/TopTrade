namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.Controllers;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Stock;

    public class StockController : BaseApiController
    {
        private readonly IAlphaVantageApiClientService stockService;
        private readonly IUserDashboardService userDashboardService;

        public StockController(
            IAlphaVantageApiClientService stockService,
            IUserDashboardService userDashboardService)
        {
            this.stockService = stockService;
            this.userDashboardService = userDashboardService;
        }

        [HttpPost]
        [ActionName("searchList")]
        public async Task<ActionResult<StockSearchResultViewModel[]>> StockSearch(StockTickerInputModel input)
        {
            return await this.stockService.SearchStockBySymbol(input.Ticker);
        }

        [HttpPost]
        [ActionName("searchStock")]
        public async Task<ActionResult<StockViewModel>> GetStockByTicker(StockSearchResultViewModel stock)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.userDashboardService.UpdateUserWatchlistAsync(stock, userId);
            var stockData = await this.stockService.GetStockByTicker(stock.Ticker);

            return this.Ok(stockData);
        }

        [HttpPost]
        [ActionName("sell")]
        public async Task<IActionResult> SellStock(StockTradeDetailsInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                var resultViewModel = await this.userDashboardService.Trade(input, userId);
                return this.Ok(resultViewModel);
            }
            catch (Exception error)
            {
                return this.Json(error.Message);
            }
        }

        [HttpPost]
        [ActionName("buy")]
        public async Task<IActionResult> BuyStock(StockTradeDetailsInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                var resultViewModel = await this.userDashboardService.Trade(input, userId);
                return this.Ok(resultViewModel);
            }
            catch (Exception error)
            {
                return this.Json(error.Message);
            }
        }

        [HttpPost]
        [ActionName("remove")]
        public async Task<IActionResult> RemoveStock(StockTickerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.userDashboardService.RemoveStockFromWatchlistAsync(input.Ticker, userId);
                return this.Ok();

            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        [ActionName("buyPercent")]
        public ActionResult<StockBuyPercentTradesViewModel> GetBuyPercent(StockTickerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            try
            {
                var stockBuyPercentViewModel = this.userDashboardService.GetStockBuyPercentTrades(input.Ticker);
                return this.Ok(stockBuyPercentViewModel);

            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }
    }
}
