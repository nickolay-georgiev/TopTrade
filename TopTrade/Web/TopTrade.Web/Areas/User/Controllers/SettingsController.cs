namespace TopTrade.Web.Areas.User.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class SettingsController : BaseLoggedUserController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
