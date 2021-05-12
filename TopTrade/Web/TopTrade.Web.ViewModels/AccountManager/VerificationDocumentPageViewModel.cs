namespace TopTrade.Web.ViewModels.AccountManager
{
    using System.Collections.Generic;

    using TopTrade.Web.ViewModels.User;

    public class VerificationDocumentPageViewModel : PagingViewModel
    {
        public ICollection<VerificationDocumentViewModel> VerificationDocuments { get; set; }
    }
}
