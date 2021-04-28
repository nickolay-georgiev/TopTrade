namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.Controllers;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class DepositController : BaseLoggedUserController
    {
        [HttpPost]
        public object MakeDeposit(DepositModalInputModel input)
        {
            //this.TempData["Error"] = "Error!";
            //this.TempData["Message"] = "Invalid Data!";

            this.TempData["Success"] = "Success!";
            this.TempData["Message"] = "Your account balance was updated!";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
