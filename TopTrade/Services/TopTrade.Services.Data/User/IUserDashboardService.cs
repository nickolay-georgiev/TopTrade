namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.Stock;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public interface IUserDashboardService
    {
        Task<UserDashboardViewModel> GetUserDataAsync(string userId);

        Task InitializeUserCollectionsAsync(ApplicationUser user);

        Task UpdateUserWatchlistAsync(StockSearchResultViewModel stock, string userId);

        Task UpdateUserAccountAsync(DepositModalInputModel input, string userId);

        Task<UserStatisticViewModel> Trade(StockTradeDetailsInputModel input, string userId);

        WithdrawViewModel GetAvailableUserWithdrawData(string userId);

        Task AcceptWithdrawRequest(WithdrawInputModel input, string userId);

        Task RemoveStockFromWatchlistAsync(string input, string userId);

        StockBuyPercentTradesViewModel GetStockBuyPercentTrades(string ticker);
    }
}
