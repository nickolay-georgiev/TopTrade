namespace TopTrade.Data.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TopTrade.Data.Common.Models;

    public class AccountStatistic : BaseDeletableModel<int>
    {
        [Column(TypeName = "decimal(14, 4)")]
        public decimal Profit { get; set; }

        [Column(TypeName = "decimal(14, 4)")]
        public decimal Available { get; set; }

        [Column(TypeName = "decimal(14, 4)")]
        public decimal TotalAllocated { get; set; }

        [Column(TypeName = "decimal(14, 4)")]
        public decimal Equity { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
