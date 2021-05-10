namespace TopTrade.Services.Data.User
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using TopTrade.Common;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.Stock;

    public class TradeService : ITradeService
    {
        private readonly IDeletableEntityRepository<Trade> tradeRepository;
        private readonly IDeletableEntityRepository<Stock> stockRepository;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatisticRepository;

        public TradeService(
            IDeletableEntityRepository<Trade> tradeRepository,
            IDeletableEntityRepository<Stock> stockRepository,
            IDeletableEntityRepository<AccountStatistic> accountStatisticRepository)
        {
            this.tradeRepository = tradeRepository;
            this.stockRepository = stockRepository;
            this.accountStatisticRepository = accountStatisticRepository;
        }

        public TradeHistoryViewModel GetTradeHistory(ApplicationUser user, int pageNumber, int itemsPerPage)
        {
            var historyViewModel = new TradeHistoryViewModel
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.tradeRepository.AllAsNoTracking().Where(x => x.UserId == user.Id).Count(),
            };

            historyViewModel.Trades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == user.Id && x.CloseDate != null)
                .OrderByDescending(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<TradeInHistoryViewModel>()
                .ToList();

            var tradesByMonths = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == user.Id && x.CloseDate != null)
                .ToList()
                .GroupBy(x => x.CloseDate.Value.Month)
                .Select(x => new TradesByMonthsChartViewModel
                {
                    Month = Enum.Parse(typeof(Month), x.Key.ToString()).ToString(),
                    BuyCount = x.Select(t => t.TradeType).ToList()
                    .Count(t => t == TradeType.BUY.ToString()),
                    SellCount = x.Select(t => t.TradeType).ToList()
                    .Count(t => t == TradeType.SELL.ToString()),
                }).ToList();

            var monthlyBalance = this.tradeRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == user.Id && x.TradeStatus == TradeStatus.CLOSE.ToString())
               .ToList()
               .GroupBy(x => x.CloseDate.Value.Month)
               .Select(x => new MonthlyWalletPerformanceChartViewModel
               {
                   Month = Enum.Parse(typeof(Month), x.Key.ToString()).ToString(),
                   Balance = (decimal)x.Select(t => t.Balance).ToList().Sum(),
               }).ToList();

            historyViewModel.ProfitLossBarChartViewModel = JsonSerializer.Serialize(tradesByMonths);
            historyViewModel.MonthlyWalletPerformanceChartViewModel = JsonSerializer.Serialize(monthlyBalance);

            return historyViewModel;
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
                throw new ArgumentException(GlobalConstants.InsuffisientFunds);
            }

            if (!Enum.IsDefined(typeof(TradeType), input.TradeType.ToUpper()))
            {
                throw new InvalidOperationException(GlobalConstants.TryAgain);
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
                CloseDate = DateTime.UtcNow,
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

        public async Task CloseTradeAsync(int id, CloseTradeInputModel input, string userId)
        {
            var trade = this.tradeRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (trade == null)
            {
                throw new ArgumentNullException(GlobalConstants.InvalidTradeId);
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

        public async Task TakeAllSwapFeesAsync()
        {
            var userGroups = this.tradeRepository
                .All()
                .Where(x => x.TradeStatus == TradeStatus.OPEN.ToString())
                .ToList()
                .GroupBy(x => x.UserId);

            foreach (var group in userGroups)
            {
                var userId = group.Key;

                foreach (var trade in group)
                {
                    trade.SwapFee += trade.OpenPrice * GlobalConstants.SwapFee;
                    this.tradeRepository.Update(trade);
                }

                var totalSwapFee = group.Sum(x => x.OpenPrice * GlobalConstants.SwapFee);
                var currentUserStatistic = this.accountStatisticRepository.All()
                    .FirstOrDefault(x => x.UserId == userId);

                currentUserStatistic.Available -= totalSwapFee;
                this.accountStatisticRepository.Update(currentUserStatistic);
            }

            await this.tradeRepository.SaveChangesAsync();
            await this.accountStatisticRepository.SaveChangesAsync();
        }
    }
}
