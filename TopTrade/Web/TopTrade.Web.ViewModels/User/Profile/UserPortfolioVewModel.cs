namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User.Stock;

    public class UserPortfolioVewModel
    {
        public ICollection<TradeInPortfolioViewModel> Trades { get; set; }

        public UserStatisticViewModel UserStatistic { get; set; }
    }
}
