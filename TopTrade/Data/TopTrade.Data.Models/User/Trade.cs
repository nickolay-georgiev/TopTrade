namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using TopTrade.Data.Common.Models;
    using TopTrade.Data.Models.User.Enums;

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

        [NotMapped]
        public decimal TotalSpent => this.Quantity * this.OpenPrice;
    }
}
