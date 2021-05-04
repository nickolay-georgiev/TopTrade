namespace TopTrade.Web.ViewModels.User.Stock
{
    using System.ComponentModel.DataAnnotations;

    public class StockTickerInputModel
    {
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string Ticker { get; set; }
    }
}
