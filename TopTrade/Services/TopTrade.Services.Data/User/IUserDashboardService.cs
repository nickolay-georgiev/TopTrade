namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Web.ViewModels.User;

    public interface IUserDashboardService
    {
        UserDashboardViewModel GetUserData(string userId);

        Task PopulateDataAsync(ApplicationUser user);

        Task UpdateUserWatchlistAsync(StockViewModel stock, string userId);
    }
}
