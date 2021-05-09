namespace TopTrade.Data.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TopTrade.Data.Common.Models;

    public class Deposit : BaseDeletableModel<int>
    {
        [Required]
        [Range(10, 50_000)]
        [Column(TypeName = "decimal(10, 4)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Currency { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string PaymentMethod { get; set; }

        public int CardId { get; set; }

        public virtual Card Card { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string TransactionStatus { get; set; }
    }
}
