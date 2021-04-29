namespace TopTrade.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserDashboardViewModel
    {
        public decimal Profit { get; set; }

        public decimal Available { get; set; }

        public decimal TotalAllocated { get; set; }

        public decimal Equity { get; set; }

        public int UserId { get; set; }

        public ICollection<StockViewModel> StockWatchlist { get; set; } = new List<StockViewModel>();
    }
}
