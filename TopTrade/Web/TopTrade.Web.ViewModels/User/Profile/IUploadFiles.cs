namespace TopTrade.Web.ViewModels.User
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public interface IUploadFiles
    {
        public IList<IFormFile> Documents { get; set; }
    }
}
