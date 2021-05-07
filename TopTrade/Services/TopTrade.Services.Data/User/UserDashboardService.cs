namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.Stock;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class UserDashboardService : IUserDashboardService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Card> cardRepository;
        private readonly IDeletableEntityRepository<Stock> stockRepository;
        private readonly IDeletableEntityRepository<Trade> tradeRepository;
        private readonly IDeletableEntityRepository<Deposit> depositRepository;
        private readonly IDeletableEntityRepository<Watchlist> watchlistRepository;
        private readonly IDeletableEntityRepository<Withdraw> withdrawRepository;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatisticRepository;
        private readonly IAlphaVantageApiClientService stockService;

        public UserDashboardService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Card> cardRepository,
            IDeletableEntityRepository<Stock> stockRepository,
            IDeletableEntityRepository<Trade> tradeRepository,
            IDeletableEntityRepository<Deposit> depositRepository,
            IDeletableEntityRepository<Watchlist> watchlistRepository,
            IDeletableEntityRepository<Withdraw> withdrawRepository,
            IDeletableEntityRepository<AccountStatistic> accountStatisticRepository,
            IAlphaVantageApiClientService stockService)
        {
            this.userManager = userManager;
            this.stockRepository = stockRepository;
            this.tradeRepository = tradeRepository;
            this.cardRepository = cardRepository;
            this.depositRepository = depositRepository;
            this.watchlistRepository = watchlistRepository;
            this.withdrawRepository = withdrawRepository;
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
                var totalTrades = this.GetStockBuyPercentTrades(stock.Ticker);
                var stockData = await this.stockService.GetStockByTicker(stock.Ticker);
                var tradeStatus = this.tradeRepository
                    .AllAsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Any(x => x.Stock.Ticker == stock.Ticker && x.TradeStatus == TradeStatus.OPEN.ToString())
                    ? TradeStatus.OPEN.ToString() : TradeStatus.CLOSE.ToString();

                stock.Change = stockData.Change;
                stock.ChangePercent = stockData.ChangePercent;
                stock.Price = stockData.Price;
                stock.BuyPercent = totalTrades.TotalBuyPercentTrades;
                stock.LogoName = string.Join(string.Empty, stock.Name.Split(new char[] { ' ', '.' })[0]);
                stock.TradeStatus = tradeStatus;
            }

            var userAccountStatistic = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<UserStatisticViewModel>()
                .FirstOrDefault();

            var dashboardViewModel = new UserDashboardViewModel
            {
                StockWatchlist = userWatchlistStocks,
                UserStatistic = userAccountStatistic,
            };

            return dashboardViewModel;
        }

        public async Task InitializeUserCollectionsAsync(ApplicationUser user)
        {
            var initialStatistic = new AccountStatistic { UserId = user.Id };
            var initialWatchlist = new Watchlist { UserId = user.Id };

            user.AccountStatistic = initialStatistic;
            user.Watchlist = initialWatchlist;

            await this.userManager.UpdateAsync(user);
        }

        public async Task UpdateUserWatchlistAsync(StockSearchResultViewModel stock, string userId)
        {
            var currentStock = this.stockRepository
                .All()
                .FirstOrDefault(x => x.Ticker == stock.Ticker);

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

            var watchlistStock = this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.Stocks)
                .ToList();

            if (watchlistStock.All(x => x.StockId != currentStock.Id))
            {
                var watchlist = this.watchlistRepository
                    .All()
                    .FirstOrDefault(x => x.UserId == userId);

                watchlist.Stocks
                    .Add(new WatchlistStocks { StockId = currentStock.Id });

                this.watchlistRepository.Update(watchlist);
                await this.watchlistRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAccountAsync(DepositModalInputModel input, string userId)
        {
            // TODO remove this split and check db
            var currentCardNumber = input.Card.Number;

            currentCardNumber =
                currentCardNumber.Substring(0, 4) + '-' + new string('*', 4) + '-' + new string('*', 4) +
                currentCardNumber.Substring(currentCardNumber.Length - 4, 4);

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

        public async Task<UserStatisticViewModel> Trade(StockTradeDetailsInputModel input, string userId)
        {
            var account = this.accountStatisticRepository
                .All()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            var totalPrice = input.Quantity * input.Price;

            if (account.Available < totalPrice)
            {
                throw new ArgumentException("Insuffisient funds to process this order");
            }

            if (!Enum.IsDefined(typeof(TradeType), input.TradeType.ToUpper()))
            {
                throw new InvalidOperationException("Something goes wrong... please try again");
            }

            account.Available -= totalPrice;
            account.TotalAllocated += totalPrice;

            this.accountStatisticRepository.Update(account);
            await this.accountStatisticRepository.SaveChangesAsync();

            var stock = this.stockRepository
                .All()
                .Where(x => x.Ticker == input.Ticker)
                .FirstOrDefault();

            var trade = new Trade
            {
                Quantity = input.Quantity,
                OpenPrice = input.Price,
                UserId = userId,
                StockId = stock.Id,
                TradeType = input.TradeType.ToUpper(),
                TradeStatus = TradeStatus.OPEN.ToString(),
            };

            await this.tradeRepository.AddAsync(trade);
            await this.tradeRepository.SaveChangesAsync();

            var resultViewModel = new UserStatisticViewModel
            {
                Available = account.Available,
                TotalAllocated = account.TotalAllocated,
                Profit = account.Profit,
            };

            return resultViewModel;
        }

        public WithdrawViewModel GetAvailableUserWithdrawData(string userId)
        {
            var withdrawViewModel = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<WithdrawViewModel>()
                .FirstOrDefault();

            return withdrawViewModel;
        }

        public async Task AcceptWithdrawRequest(WithdrawInputModel input, string userId)
        {
            if (input.Available < input.DesiredAmount)
            {
                throw new InvalidOperationException($"You can withdraw up to ${input.Available}");
            }

            var card = this.cardRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && x.Number == input.Card)
                .FirstOrDefault();

            var withdraw = new Withdraw
            {
                Amount = input.DesiredAmount,
                UserId = userId,
                CardId = card.Id,
                TransactionStatus = TransactionStatus.Pending.ToString(),
            };

            await this.withdrawRepository.AddAsync(withdraw);
            await this.withdrawRepository.SaveChangesAsync();
        }

        public async Task RemoveStockFromWatchlistAsync(string input, string userId)
        {
            var currentStock = this.stockRepository
               .All()
               .FirstOrDefault(x => x.Ticker == input);

            var watchlist = this.watchlistRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId);

            var watchlistStocks = this.watchlistRepository
                .All()
                .SelectMany(x => x.Stocks)
                .FirstOrDefault(x => x.StockId == currentStock.Id && x.WatchlistId == watchlist.Id);

            watchlist.Stocks.Remove(watchlistStocks);

            this.watchlistRepository.Update(watchlist);
            await this.watchlistRepository.SaveChangesAsync();
        }

        public StockBuyPercentTradesViewModel GetStockBuyPercentTrades(string ticker)
        {
            double totalBuyTrades = this.tradeRepository
                   .AllAsNoTracking()
                   .Where(x => x.Stock.Ticker == ticker && x.TradeType == TradeType.BUY.ToString())
                   .Count();

            double totalSellTrades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.Stock.Ticker == ticker && x.TradeType == TradeType.SELL.ToString())
                .Count();

            double totalTrades = totalBuyTrades + totalSellTrades;
            totalTrades = totalTrades == 0 ? 1 : totalTrades;

            int buyTrades = (int)Math.Round((totalBuyTrades / totalTrades) * 100);

            return new StockBuyPercentTradesViewModel { TotalBuyPercentTrades = buyTrades };
        }

        public async Task<UserPortfolioVewModel> GetPortoflioAsync(string userId)
        {
            var trades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && x.TradeStatus == TradeStatus.OPEN.ToString())
                .To<TradeInPortfolioViewModel>()
                .ToList();

            foreach (var trade in trades)
            {
                var stockData = await this.stockService.GetStockByTicker(trade.StockTicker);
                trade.CurrentPrice = stockData.Price;

                trade.ProfitLossInCash = trade.TradeType == "BUYING" ?
                trade.CurrentPrice - trade.OpenPrice : trade.OpenPrice - trade.CurrentPrice;

                trade.ProfitLossInPercent = trade.TradeType == "BUYING" ?
                (trade.CurrentPrice - trade.OpenPrice) * 100 / trade.OpenPrice :
                (trade.OpenPrice - trade.CurrentPrice) * 100 / trade.OpenPrice;
            }

            var userStatistic = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<UserStatisticViewModel>()
                .FirstOrDefault();

            var portfolioVewModel = new UserPortfolioVewModel
            {
                Trades = trades,
                UserStatistic = userStatistic,
            };

            return portfolioVewModel;
        }

        public async Task CloseTradeAsync(int id, CloseTradeInputModel input, string userId)
        {
            var trade = this.tradeRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (trade == null)
            {
                throw new ArgumentNullException("Invalid trade id");
            }

            trade.CloseDate = DateTime.UtcNow;
            trade.ClosePrice = input.CurrentPrice;
            trade.TradeStatus = TradeStatus.CLOSE.ToString();

            this.tradeRepository.Update(trade);
            await this.tradeRepository.SaveChangesAsync();

            var accountStatistic = this.accountStatisticRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId);

            var tradeOpenPriceTotal = trade.OpenPrice * trade.Quantity;
            var tradeClosePriceTotal = input.CurrentPrice * trade.Quantity;

            var profitAsAbs = Math.Abs(tradeClosePriceTotal - tradeOpenPriceTotal);
            if (trade.TradeType == TradeType.BUY.ToString())
            {
                var profit = tradeClosePriceTotal - tradeOpenPriceTotal;
                accountStatistic.Available += profit + tradeOpenPriceTotal;

                if (profit < 0)
                {
                    accountStatistic.Profit += profitAsAbs;
                }
                else
                {
                    accountStatistic.Profit -= profitAsAbs;
                }
            }
            else
            {
                var profit = tradeOpenPriceTotal - tradeClosePriceTotal;
                accountStatistic.Available += profit + tradeOpenPriceTotal;

                if (profit < 0)
                {
                    accountStatistic.Profit += profitAsAbs;
                }
                else
                {
                    accountStatistic.Profit -= profitAsAbs;
                }
            }

            accountStatistic.TotalAllocated -= trade.OpenPrice * trade.Quantity;

            this.accountStatisticRepository.Update(accountStatistic);
            await this.accountStatisticRepository.SaveChangesAsync();
        }
    }
}
