namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.Controllers;
    using TopTrade.Web.ViewModels.User;

    public class StockSearchController : BaseApiController
    {
        private readonly IAlphaVantageApiClientService stockService;
        private readonly IUserDashboardService userDashboardService;

        public StockSearchController(
            IAlphaVantageApiClientService stockService,
            IUserDashboardService userDashboardService)
        {
            this.stockService = stockService;
            this.userDashboardService = userDashboardService;
        }

        [HttpPost]
        [ActionName("list")]
        public async Task<ActionResult<StockSearchResultViewModel[]>> StockSearch([FromBody] string stockNameOrTicker)
        {
            return await this.stockService.SearchStockBySymbol(stockNameOrTicker);
        }

        [HttpPost]
        [ActionName("stock")]
        public async Task<ActionResult<StockViewModel>> GetStockByTicker(StockSearchResultViewModel stock)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var stockData = await this.stockService.GetStockByTicker(stock.Ticker);

            //var stockData = await this.stockService.GetStockByTicker(stockTicker);

            await this.userDashboardService.UpdateUserWatchlistAsync(stock, userId);
            return stockData;
        }
    }
}
