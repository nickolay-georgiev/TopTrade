namespace TopTrade.Web.ViewModels.User.Stock
{
    using System.ComponentModel.DataAnnotations;

    using TopTrade.Data.Models.User.Enums;

    public class StockTradeDetailsInputModel
    {
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string Ticker { get; set; }

        [Range(1, 100000)]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [EnumDataType(typeof(TradeType))]
        public string TradeType { get; set; }
    }
}
