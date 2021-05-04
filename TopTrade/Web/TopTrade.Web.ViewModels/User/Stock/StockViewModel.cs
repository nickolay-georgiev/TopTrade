namespace TopTrade.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StockViewModel
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public decimal Price { get; set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }

        public int BuyPercent { get; set; }

        public ICollection<decimal> DataSet { get; set; }
    }
}
