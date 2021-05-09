namespace TopTrade.Data.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TopTrade.Data.Common.Models;

    public class Trade : BaseDeletableModel<int>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(14, 2)")]
        public decimal OpenPrice { get; set; }

        [Column(TypeName = "decimal(14, 2)")]
        public decimal? ClosePrice { get; set; }

        public string TradeStatus { get; set; }

        public string TradeType { get; set; }

        public int StockId { get; set; }

        public virtual Stock Stock { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime? CloseDate { get; set; }

        public decimal? Balance { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal SwapFee { get; set; }

        [NotMapped]
        public decimal TotalSpent => this.Quantity * this.OpenPrice;
    }
}
