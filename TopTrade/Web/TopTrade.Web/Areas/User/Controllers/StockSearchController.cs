namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data;
    using TopTrade.Web.Controllers;
    using TopTrade.Web.ViewModels.User;

    public class StockSearchController : BaseApiController
    {
        private readonly IAlphaVantageApiClientService stockService;

        public StockSearchController(IAlphaVantageApiClientService stockService)
        {
            this.stockService = stockService;
        }

        [HttpPost]
        [ActionName("list")]
        public async Task<ActionResult<StockSearchResultViewModel[]>> StockSearch([FromBody] string stockNameOrTicker)
        {
            return await this.stockService.SearchStockBySymbol(stockNameOrTicker);
        }

        [HttpPost]
        [ActionName("stock")]
        public async Task<ActionResult<StockViewModel>> GetStockByTicker([FromBody] string stockTicker)
        {
            return await this.stockService.GetStockByTicker(stockTicker);
        }
    }
}
