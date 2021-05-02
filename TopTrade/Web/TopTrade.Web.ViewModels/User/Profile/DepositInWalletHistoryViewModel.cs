namespace TopTrade.Web.ViewModels.User.Profile
{
    using System;

    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class DepositInWalletHistoryViewModel : IMapFrom<Deposit>
    {
        public decimal Amount { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CardNumber { get; set; }

        public string TransactionStatus { get; set; }
    }
}
