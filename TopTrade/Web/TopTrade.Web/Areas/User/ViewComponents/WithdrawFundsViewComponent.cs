namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class WithdrawFundsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //VerificationDocumentsInputModel documentsViewModel = new VerificationDocumentsInputModel();
            return this.View();
        }
    }
}
