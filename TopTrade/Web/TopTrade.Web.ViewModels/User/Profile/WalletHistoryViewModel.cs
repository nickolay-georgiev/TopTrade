namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;

    public class WalletHistoryViewModel
    {
        public PagingViewModel DepositsPaging { get; set; }

        public IEnumerable<DepositInWalletHistoryViewModel> Deposits { get; set; }

        public PagingViewModel WithdrawPaging { get; set; }

        public IEnumerable<WithdrawInWalletHistoryViewModel> Withdraws { get; set; }
    }
}
