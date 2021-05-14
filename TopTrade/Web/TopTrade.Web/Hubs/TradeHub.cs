namespace TopTrade.Web.Areas.User.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.ViewModels.User.Stock;

    [Authorize]
    public class TradeHub : Hub
    {
        private readonly IStockService stockService;
        private readonly UserManager<ApplicationUser> userManager;

        public TradeHub(
            IStockService stockService,
            UserManager<ApplicationUser> userManager)
        {
            this.stockService = stockService;
            this.userManager = userManager;
        }

        public async Task GetUpdateForStockPrice(StockTickerInputModel[] input)
        {
            var user = await this.userManager.GetUserAsync(this.Context.User);

            var updatedStocksData = await this.stockService.GetActualStockDataAsync(user.Id, input);
            await this.Clients.Caller.SendAsync("GetStockData", updatedStocksData);
        }
    }
}
