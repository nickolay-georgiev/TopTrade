namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.Controllers;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Stock;

    public class StockController : BaseApiController
    {
        private readonly IStockService stockService;
        private readonly ITradeService tradeService;

        public StockController(
            IStockService stockService,
            ITradeService tradeService)
        {
            this.stockService = stockService;
            this.tradeService = tradeService;
        }

        [HttpPost]
        [ActionName("searchList")]
        public async Task<ActionResult<StockSearchResultViewModel[]>> StockSearch(StockTickerInputModel input)
        {
            return await this.stockService.SearchStocksBySymbolAsync(input.Ticker);
        }

        [HttpPost]
        [ActionName("closeTrade")]
        public ActionResult<StockTickerInputModel> Close(StockTickerInputModel input)
        {
            return input;
        }

        [HttpPost]
        [ActionName("searchStock")]
        public async Task<ActionResult<StockViewModel>> GetStockByTicker(StockSearchResultViewModel stock)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.stockService.UpdateUserStockWatchlistAsync(stock, userId);
            var stockData = await this.stockService.GetStockDataAsync(stock.Ticker);

            return this.Ok(stockData);
        }

        [HttpPost]
        [ActionName("trade")]
        public async Task<IActionResult> SellStock(StockTradeDetailsInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                var resultViewModel = await this.tradeService.Trade(input, userId);
                return this.Ok(resultViewModel);
            }
            catch (Exception error)
            {
                return this.Json(error.Message);
            }
        }

        [HttpPost]
        [ActionName("removeFromWathlist")]
        public async Task<IActionResult> RemoveStock(StockTickerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.stockService.RemoveStockFromWatchlistAsync(input.Ticker, userId);
                return this.Ok();
            }
            catch (Exception)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        [ActionName("buyPercent")]
        public ActionResult<StockBuyPercentTradesViewModel> GetBuyPercent(StockTickerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            try
            {
                var stockBuyPercentViewModel = this.stockService.GetStockBuyPercentTrades(input.Ticker);
                return this.Ok(stockBuyPercentViewModel);
            }
            catch (Exception)
            {
                return this.NotFound();
            }
        }
    }
}
