namespace TopTrade.Web.ViewModels.Administration
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User;

    public class UsersPageViewModel : PagingViewModel
    {
        public ICollection<UserInUsersPageViewModel> Users { get; set; }
    }
}
