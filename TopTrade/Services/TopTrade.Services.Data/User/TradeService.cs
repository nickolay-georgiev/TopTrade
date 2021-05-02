namespace TopTrade.Services.Data.User
{
    using System.Collections.Generic;
    using System.Linq;

    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User.Stock;

    public class TradeService : ITradeService
    {
        private readonly IDeletableEntityRepository<Trade> tradeRepository;

        public TradeService(IDeletableEntityRepository<Trade> tradeRepository)
        {
            this.tradeRepository = tradeRepository;
        }

        public TradeHistoryViewModel GetTradeHistory(string userId, int pageNumber, int itemsPerPage = 8)
        {
            var historyViewModel = new TradeHistoryViewModel
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.tradeRepository.AllAsNoTracking().Where(x => x.UserId == userId).Count(),
            };

            historyViewModel.Trades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<TradeInHistoryViewModel>()
                .ToList();

            return historyViewModel;
        }
    }
}
