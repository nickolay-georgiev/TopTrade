namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
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
        public async Task<ActionResult<StockSearchResultViewModel[]>> StockSearch([FromBody] string stockNameOrTicker)
        {
            return await this.stockService.SearchStockBySymbol(stockNameOrTicker);
        }

        [HttpPost]
        [ActionName("searchStock")]
        public async Task<ActionResult<StockViewModel>> GetStockByTicker(StockSearchResultViewModel stock)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var stockData = await this.stockService.GetStockByTicker(stock.Ticker);

            await this.userDashboardService.UpdateUserWatchlistAsync(stock, userId);
            return stockData;
        }


        [HttpPost]
        [ActionName("sell")]
        public async Task<ActionResult> SellStock(StockTradeDetailsInputModel input)
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
        public async Task<ActionResult> BuyStock(StockTradeDetailsInputModel input)
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
    }
}
