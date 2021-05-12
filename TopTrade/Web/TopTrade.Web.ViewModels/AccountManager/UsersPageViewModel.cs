namespace TopTrade.Web.ViewModels.AccountManager
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User;

    public class UsersPageViewModel : PagingViewModel
    {
        public ICollection<UserInPageViewModel> Users { get; set; }
    }
}
