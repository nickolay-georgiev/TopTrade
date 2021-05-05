namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.User;

    public class PortfolioController : BaseLoggedUserController
    {
        private readonly IUserDashboardService userDashboardService;

        public PortfolioController(IUserDashboardService userDashboardService)
        {
            this.userDashboardService = userDashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var portfolioVewModel = this.userDashboardService.GetPortoflio(userId);

            return this.View(portfolioVewModel);
        }
    }
}
