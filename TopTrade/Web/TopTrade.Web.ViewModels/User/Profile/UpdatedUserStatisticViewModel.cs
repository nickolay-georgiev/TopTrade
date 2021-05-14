namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User.Stock;

    public class UpdatedUserStatisticViewModel
    {
        public ICollection<ActualStockDataViewModel> Stocks { get; set; }

        public decimal UserProfit { get; set; }

        public decimal UserEquity { get; set; }
    }
}
