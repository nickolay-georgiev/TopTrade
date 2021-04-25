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
        public TradeType TradeType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public DateTime ExecutionTime { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [NotMapped]
        public decimal TotalSpent => this.Quantity * this.Price;
    }
}
