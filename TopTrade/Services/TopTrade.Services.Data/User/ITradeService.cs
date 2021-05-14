namespace TopTrade.Services.Data.User
{
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.Stock;

    public interface ITradeService
    {
        TradeHistoryViewModel GetTradeHistory(string userId, int page, int itemsPerPage);

        Task TakeAllSwapFeesAsync();

        Task<UserStatisticViewModel> OpenTradeAsync(StockTradeDetailsInputModel input, string userId);

        Task CloseTradeAsync(int id, CloseTradeInputModel input, string userId);
    }
}
