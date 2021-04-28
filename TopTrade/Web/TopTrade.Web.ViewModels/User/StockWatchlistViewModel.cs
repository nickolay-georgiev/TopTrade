namespace TopTrade.Web.ViewModels.User
{
    public class StockWatchlistViewModel
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public decimal Price { get; set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }
    }
}
