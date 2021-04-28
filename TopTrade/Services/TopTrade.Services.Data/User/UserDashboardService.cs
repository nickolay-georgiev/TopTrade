namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Web.ViewModels.User;

    public class UserDashboardService : IUserDashboardService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Stock> stockRepository;
        private readonly IDeletableEntityRepository<Watchlist> watchlistRepository;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatisticRepository;

        public UserDashboardService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Stock> stockRepository,
            IDeletableEntityRepository<Watchlist> watchlistRepository,
            IDeletableEntityRepository<AccountStatistic> accountStatisticRepository)
        {
            this.userManager = userManager;
            this.stockRepository = stockRepository;
            this.watchlistRepository = watchlistRepository;
            this.accountStatisticRepository = accountStatisticRepository;
        }

        public UserDashboardViewModel GetUserData(string userId)
        {
            var userWatchlistStocks = this.watchlistRepository
                 .AllAsNoTracking()
                 .Where(x => x.UserId == userId)
                 .SelectMany(x => x.Stocks)
                 .Select(x => new StockWatchlistViewModel
                 {
                     Name = x.Stock.Name,
                     Ticker = x.Stock.Ticker,
                 }).ToList();

            var userAccountStatistic = this.accountStatisticRepository
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

        public async Task PopulateDataAsync(ApplicationUser user)
        {
            var initialStatistic = new AccountStatistic { UserId = user.Id };
            var initialWatchlist = new Watchlist { UserId = user.Id };

            user.AccountStatistic = initialStatistic;
            user.Watchlist = initialWatchlist;

            await this.userManager.UpdateAsync(user);
        }

        public async Task UpdateUserWatchlistAsync(StockViewModel stock, string userId)
        {
            //var stockExist = this.stockRepository
            //    .All()
            //    .Where(x => x.Ticker == stock.Ticker)
            //    .FirstOrDefault();

            //var currentStock = new Stock
            //{
            //    Name = stock.Name,
            //    Ticker = stock.Ticker,
            //};

            //if (stockExist == null)
            //{
            //    await this.stockRepository.AddAsync(currentStock);
            //    await this.stockRepository.SaveChangesAsync();
            //    this.stockRepository.Dispose();
            //}

            var currentStock = new Stock
            {
                Name = stock.Name,
                Ticker = stock.Ticker,
            };

            var watchlist = this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            if (watchlist.Stocks.All(x => x.Stock.Ticker != stock.Ticker))
            {
                watchlist.Stocks.Add(new WatchlistStocks { Stock = currentStock });
                this.watchlistRepository.Update(watchlist);
            }

            await this.watchlistRepository.SaveChangesAsync();
        }
    }
}
