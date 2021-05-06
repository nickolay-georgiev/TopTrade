namespace TopTrade.Web.ViewModels.User
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User.Profile;

    public class UserDashboardViewModel
    {
        public UserStatisticViewModel UserStatistic { get; set; }

        public ICollection<StockViewModel> StockWatchlist { get; set; }
    }
}
