namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class BaseUploadFileInputModel : IUploadFiles
    {
        public BaseUploadFileInputModel()
        {
            this.Documents = new List<IFormFile>();
        }

        public IList<IFormFile> Documents { get; set; }
    }
}
