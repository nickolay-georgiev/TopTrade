namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        public IEnumerable<TradeHistoryViewModel> GetTradeHistory(string userId, int page, int itemsPerPage = 8)
        {
            var historyViewModel = new TradeHistoryViewModel();

            var userTrades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<TradeHistoryViewModel>()
                .ToList();

            if (userTrades == null)
            {

            }

            return userTrades;
        }
    }
}
