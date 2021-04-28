namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TopTrade.Web.ViewModels.User;

    public interface IUserDashboardService
    {
        UserDashboardViewModel GetUserData(string userId);
    }
}
