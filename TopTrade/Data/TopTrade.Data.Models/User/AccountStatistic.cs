namespace TopTrade.Data.Models.User
{
    using TopTrade.Data.Common.Models;

    public class AccountStatistic : BaseDeletableModel<int>
    {
        public decimal Profit { get; set; }

        public decimal Available { get; set; }

        public decimal TotalAllocated { get; set; }

        public decimal Equity { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
