namespace TopTrade.Web.ViewModels.User.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserStatisticViewModel
    {
        public decimal Profit { get; set; }

        public decimal Available { get; set; }

        public decimal TotalAllocated { get; set; }

        public decimal Equity => this.Profit + this.Available + this.TotalAllocated;
    }
}
