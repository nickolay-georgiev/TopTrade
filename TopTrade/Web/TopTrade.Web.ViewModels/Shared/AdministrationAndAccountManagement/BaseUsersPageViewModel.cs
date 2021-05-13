namespace TopTrade.Web.ViewModels.Shared
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User;

    public class BaseUsersPageViewModel<T> : PagingViewModel
    {
        public ICollection<T> Users { get; set; }
    }
}
