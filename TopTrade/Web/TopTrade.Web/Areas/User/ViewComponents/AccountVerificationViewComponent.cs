namespace TopTrade.Web.Areas.User.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.ViewModels.User;

    public class AccountVerificationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var documentsViewModel = new VerificationDocumentsInputModel();
            return this.View(documentsViewModel);
        }
    }
}
