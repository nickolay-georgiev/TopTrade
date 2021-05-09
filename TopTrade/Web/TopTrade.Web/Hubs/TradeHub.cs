namespace TopTrade.Web.Areas.User.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class TradeHub : Hub
    {

        public TradeHub()
        {
            
        }

        public async Task GetActualStockData(string[] data)
        {
            await this.Clients.Caller.SendAsync("GetStockData", data);
        }
    }
}
