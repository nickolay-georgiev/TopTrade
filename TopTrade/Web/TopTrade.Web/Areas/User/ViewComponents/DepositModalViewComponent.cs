namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class DepositModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            DepositModalInputModel depositViewModel = new DepositModalInputModel();
            return this.View(depositViewModel);
        }
    }
}
