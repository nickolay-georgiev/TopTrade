namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using TopTrade.Data.Common.Models;
    using TopTrade.Data.Models.User.Enums;

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
