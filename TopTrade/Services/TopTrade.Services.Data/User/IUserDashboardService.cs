﻿namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public interface IUserDashboardService
    {
        Task<UserDashboardViewModel> GetUserDataAsync(string userId);

        Task InitializeUserCollectionsAsync(ApplicationUser user);

        Task UpdateUserWatchlistAsync(StockSearchResultViewModel stock, string userId);

        Task UpdateUserAccountAsync(DepositModalInputModel input, string userId);
    }
}
