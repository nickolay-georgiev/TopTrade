namespace TopTrade.Web.ViewModels.User.Stock
{
    public class ActualStockDataViewModel
    {
        public string Ticker { get; set; }

        public decimal Price { get; set; }

        public decimal Change { get; set; }

        public decimal ChangePercent { get; set; }
    }
}
