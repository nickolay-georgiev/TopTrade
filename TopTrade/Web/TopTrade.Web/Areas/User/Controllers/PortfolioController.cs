namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class PortfolioController : BaseLoggedUserController
    {
        public async Task<IActionResult> Index()
        {
            //var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var userData = await this.userDashboardService.GetUserDataAsync(userId);
            return this.View();
        }
    }
}
