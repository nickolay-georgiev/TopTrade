namespace TopTrade.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class VerificationDocumentsInputModel
    {
        public VerificationDocumentsInputModel()
        {
            this.Documents = new List<IFormFile>();
        }

        public IList<IFormFile> Documents { get; set; }
    }
}
