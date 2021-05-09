namespace TopTrade.Data.Models.User
{
    using System.ComponentModel.DataAnnotations.Schema;

    using TopTrade.Data.Common.Models;

    public class Withdraw : BaseDeletableModel<int>
    {
        [Column(TypeName = "decimal(14, 2)")]
        public decimal Amount { get; set; }

        public int CardId { get; set; }

        public Card Card { get; set; }

        public string UserId { get; set; }

        public string TransactionStatus { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
