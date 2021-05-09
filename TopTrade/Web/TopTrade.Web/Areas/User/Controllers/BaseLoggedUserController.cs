namespace TopTrade.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("User")]
    [Authorize]
    public class BaseLoggedUserController : Controller
    {
    }
}
