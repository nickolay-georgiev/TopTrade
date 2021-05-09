namespace TopTrade.Web.ViewModels.User.Stock
{
    public class StockTradeDetailsInputModel
    {
        public string Ticker { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string TradeType { get; set; }
    }
}
