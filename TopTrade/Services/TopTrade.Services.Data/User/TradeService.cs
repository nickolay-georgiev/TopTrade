namespace TopTrade.Services.Data.User
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User.Stock;

    public class TradeService : ITradeService
    {
        private readonly IDeletableEntityRepository<Trade> tradeRepository;

        public TradeService(IDeletableEntityRepository<Trade> tradeRepository)
        {
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
    }
}
