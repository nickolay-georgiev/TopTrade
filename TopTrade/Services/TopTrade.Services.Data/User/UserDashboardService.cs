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
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class UserDashboardService : IUserDashboardService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Card> cardRepository;
        private readonly IDeletableEntityRepository<Stock> stockRepository;
        private readonly IDeletableEntityRepository<Deposit> depositRepository;
        private readonly IDeletableEntityRepository<Watchlist> watchlistRepository;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatisticRepository;
        private readonly IAlphaVantageApiClientService stockService;

        public UserDashboardService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Card> cardRepository,
            IDeletableEntityRepository<Stock> stockRepository,
            IDeletableEntityRepository<Deposit> depositRepository,
            IDeletableEntityRepository<Watchlist> watchlistRepository,
            IDeletableEntityRepository<AccountStatistic> accountStatisticRepository,
            IAlphaVantageApiClientService stockService)
        {
            this.userManager = userManager;
            this.stockRepository = stockRepository;
            this.cardRepository = cardRepository;
            this.depositRepository = depositRepository;
            this.watchlistRepository = watchlistRepository;
            this.accountStatisticRepository = accountStatisticRepository;
            this.stockService = stockService;
        }

        public async Task<UserDashboardViewModel> GetUserDataAsync(string userId)
        {
            var userWatchlistStocks = this.watchlistRepository
                 .AllAsNoTracking()
                 .Where(x => x.UserId == userId)
                 .SelectMany(x => x.Stocks)
                 .Select(x => new StockViewModel
                 {
                     Name = x.Stock.Name,
                     Ticker = x.Stock.Ticker,
                 }).ToList();

            foreach (var stock in userWatchlistStocks)
            {
                var stockData = await this.stockService.GetStockByTicker(stock.Ticker);
                stock.Change = stockData.Change;
                stock.ChangePercent = stockData.ChangePercent;
                stock.Price = stockData.Price;
            }

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

        public async Task InitializeUserCollectionsAsync(ApplicationUser user)
        {
            var initialStatistic = new AccountStatistic { UserId = user.Id };
            var initialWatchlist = new Watchlist { UserId = user.Id };

            //user.Deposits = new List<Deposit>();
            user.AccountStatistic = initialStatistic;
            user.Watchlist = initialWatchlist;

            await this.userManager.UpdateAsync(user);
        }

        public async Task UpdateUserWatchlistAsync(StockSearchResultViewModel stock, string userId)
        {
            var currentStock = this.stockRepository
                .All()
                .Where(x => x.Ticker == stock.Ticker)
                .FirstOrDefault();

            if (currentStock == null)
            {
                currentStock = new Stock
                {
                    Name = stock.Name,
                    Ticker = stock.Ticker,
                };

                await this.stockRepository.AddAsync(currentStock);
                await this.stockRepository.SaveChangesAsync();
            }

            var watchlist = this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            if (watchlist.Stocks.All(x => x.Stock.Ticker != stock.Ticker))
            {
                watchlist.Stocks.Add(new WatchlistStocks { StockId = currentStock.Id });
                this.watchlistRepository.Update(watchlist);
                await this.watchlistRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAccountAsync(DepositModalInputModel input, string userId)
        {
            var currentCardNumber = input.Card.Number.Replace("-", "");

            var card = this.cardRepository
                .All()
                .Where(x => x.UserId == userId && x.Number == currentCardNumber)
                .FirstOrDefault();

            if (card == null)
            {
                card = new Card
                {
                    Number = currentCardNumber,
                    UserId = userId,
                };

                _ = this.cardRepository.AddAsync(card);
                await this.cardRepository.SaveChangesAsync();
            }

            _ = this.depositRepository
                .AddAsync(new Deposit
                {
                    Amount = input.Amount,
                    Currency = "USD",
                    PaymentMethod = "Credit/Debit Card",
                    UserId = userId,
                    CardId = card.Id,
                    TransactionStatus = TransactionStatus.Completed.ToString(),
                });

            await this.depositRepository.SaveChangesAsync();

            var currentUserAccoutStatistic = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            currentUserAccoutStatistic.Equity += input.Amount;
            currentUserAccoutStatistic.Available += input.Amount;

            this.accountStatisticRepository.Update(currentUserAccoutStatistic);
            await this.accountStatisticRepository.SaveChangesAsync();
        }
    }
}
