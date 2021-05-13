namespace TopTrade.Web.ViewModels.Administration
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using TopTrade.Web.ViewModels.User;

    public class AccountManagerPageViewModel : PagingViewModel
    {
        public ICollection<AccountManagerViewModel> Managers { get; set; }
    }
}
