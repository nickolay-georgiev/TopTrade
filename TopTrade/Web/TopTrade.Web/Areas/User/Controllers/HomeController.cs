namespace TopTrade.Web.Areas.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.Areas.User.Controllers;
    using TopTrade.Web.Controllers;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class HomeController : BaseLoggedUserController
    {
        private readonly IAlphaVantageApiClientService stockService;
        private readonly IUserDashboardService userDashboardService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IAlphaVantageApiClientService stockService,
            IUserDashboardService userDashboardService,
            UserManager<ApplicationUser> userManager)
        {
            this.stockService = stockService;
            this.userDashboardService = userDashboardService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userData = this.userDashboardService.GetUserData(userId);

            return this.View(userData);
        }
    }
}

//  var getStockByTicker = await this.stockService.GetStockTimeSeries();
//  var getStockByTicker1 = await this.stockService.GetStockByTicker1();
//  var searchStock = await this.stockService.SearchStock();