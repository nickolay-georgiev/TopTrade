namespace TopTrade.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.User;

    public interface IUploadDocumentsService
    {
        Task UploadDocumentsAsync(IUploadFiles input, string userId, string imagePath, [CallerMemberName] string callerName = "");
    }
}
