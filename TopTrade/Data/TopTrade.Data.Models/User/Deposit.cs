namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using TopTrade.Data.Common.Models;
    using TopTrade.Data.Models.User.Enums;

    public class Deposit : BaseDeletableModel<int>
    {
        [Required]
        [Range(10, 50_000)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Currency { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string PaymentMethod { get; set; }

        public DateTime Date { get; set; }

        public int CardId { get; set; }

        public virtual Card Card { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string TransactionStatus { get; set; }
    }
}
