namespace TopTrade.Web.ViewModels.AccountManager
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TopTrade.Web.ViewModels.User;

    public class WithdrawRequestPageViewModel : PagingViewModel
    {
        public ICollection<WithdrawRequestViewModel> WithdrawRequestViewModels { get; set; }
    }
}
