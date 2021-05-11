namespace TopTrade.Web.Areas.User.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.ViewModels.User.Stock;

    [Authorize]
    public class TradeHub : Hub
    {
        private readonly IStockService stockService;

        public TradeHub(IStockService stockService)
        {
            this.stockService = stockService;
        }

        public async Task GetUpdateForStockPrice(StockTickerInputModel[] input)
        {
            var updatedStocksData = await this.stockService.GetActualStockDataAsync(input);
            await this.Clients.Caller.SendAsync("GetStockData", updatedStocksData);
        }
    }
}
