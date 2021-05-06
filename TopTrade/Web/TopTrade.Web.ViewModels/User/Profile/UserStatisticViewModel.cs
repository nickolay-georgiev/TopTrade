namespace TopTrade.Web.ViewModels.User.Profile
{
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class UserStatisticViewModel : IMapFrom<AccountStatistic>
    {
        public decimal Profit { get; set; }

        public decimal Available { get; set; }

        public decimal TotalAllocated { get; set; }

        public decimal Equity => this.Profit + this.Available + this.TotalAllocated;
    }
}
