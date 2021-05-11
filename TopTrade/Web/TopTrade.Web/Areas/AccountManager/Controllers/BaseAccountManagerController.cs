namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Common;

    [Area("AccountManager")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class BaseAccountManagerController : Controller
    {
    }
}
