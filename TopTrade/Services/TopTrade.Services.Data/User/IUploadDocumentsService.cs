namespace TopTrade.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.User;

    public interface IUploadDocumentsService
    {
        Task UploadDocumentsAsync(VerificationDocumentsInputModel input, string imagePath);
    }
}
