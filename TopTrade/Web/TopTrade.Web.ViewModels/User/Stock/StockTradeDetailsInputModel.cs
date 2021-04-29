namespace TopTrade.Web.ViewModels.User.Stock
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StockTradeDetailsInputModel
    {
        public string Ticker { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string TradeType { get; set; }
    }
}
