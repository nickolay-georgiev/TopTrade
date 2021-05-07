namespace TopTrade.Services.Data.User
{
    using TopTrade.Data.Models;
    using TopTrade.Web.ViewModels.User.Stock;

    public interface ITradeService
    {
        TradeHistoryViewModel GetTradeHistory(ApplicationUser user, int page, int itemsPerPage);
    }
}
