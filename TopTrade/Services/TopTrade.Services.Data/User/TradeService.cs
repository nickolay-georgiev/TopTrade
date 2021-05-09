namespace TopTrade.Services.Data.User
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User.Stock;

    public class TradeService : ITradeService
    {
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatistic;
        private readonly IDeletableEntityRepository<Trade> tradeRepository;

        public TradeService(
            IDeletableEntityRepository<AccountStatistic> accountStatistic,
            IDeletableEntityRepository<Trade> tradeRepository)
        {
            this.accountStatistic = accountStatistic;
            this.tradeRepository = tradeRepository;
        }

        public TradeHistoryViewModel GetTradeHistory(ApplicationUser user, int pageNumber, int itemsPerPage = 8)
        {
            var historyViewModel = new TradeHistoryViewModel
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.tradeRepository.AllAsNoTracking().Where(x => x.UserId == user.Id).Count(),
            };

            historyViewModel.Trades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == user.Id)
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
               .Where(x => x.UserId == user.Id && x.TradeStatus == "CLOSE")
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
                    trade.SwapFee += trade.OpenPrice * 0.01m;
                    this.tradeRepository.Update(trade);
                }


                var totalSwapFee = group.Sum(x => x.OpenPrice * 0.01m);
                var currentUserStatistic = this.accountStatistic.All()
                    .FirstOrDefault(x => x.UserId == userId);

                currentUserStatistic.Available -= totalSwapFee;
                this.accountStatistic.Update(currentUserStatistic);
            }

            await this.tradeRepository.SaveChangesAsync();
            await this.accountStatistic.SaveChangesAsync();
        }

        public string GetData()
        {
            return "test data";
        }
    }
}
