namespace TopTrade.Web.ViewModels.User
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class UserAvatarInputModel : IUploadFiles
    {
        public UserAvatarInputModel()
        {
            this.Documents = new List<IFormFile>();
        }

        public IList<IFormFile> Documents { get; set; }
    }
}
