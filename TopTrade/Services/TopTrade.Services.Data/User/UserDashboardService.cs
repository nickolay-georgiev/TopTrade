namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models.User;
    using TopTrade.Web.ViewModels.User;

    public class UserDashboardService : IUserDashboardService
    {
        private readonly IDeletableEntityRepository<Watchlist> watchlist;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatistic;

        public UserDashboardService(
            IDeletableEntityRepository<Watchlist> watchlist,
            IDeletableEntityRepository<AccountStatistic> accountStatistic)
        {
            this.watchlist = watchlist;
            this.accountStatistic = accountStatistic;
        }

        public UserDashboardViewModel GetUserData(string userId)
        {
            var userWatchlistStocks = this.watchlist
                 .AllAsNoTracking()
                 .Where(x => x.UserId == userId)
                 .SelectMany(x => x.Stocks)
                 .Select(x => new StockWatchlistViewModel
                 {
                     Name = x.Stock.Name,
                     Ticker = x.Stock.Ticker,
                     Change = x.Stock.Change,
                     ChangePercent = x.Stock.ChangePercent,
                     Price = x.Stock.Price,
                 }).ToList();

            var userAccountStatistic = this.accountStatistic
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new UserDashboardViewModel
                {
                    Profit = x.Profit,
                    Available = x.Available,
                    TotalAllocated = x.TotalAllocated,
                    Equity = x.Equity,
                    StockWatchlist = userWatchlistStocks,
                }).FirstOrDefault();

            return userAccountStatistic;
        }
    }
}
