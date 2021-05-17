namespace TopTrade.Services.Data
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.User.Profile;

    public interface IUploadDocumentsService
    {
        Task UploadDocumentsAsync(BaseUploadFileInputModel input, string userId, string imagePath, [CallerMemberName] string callerName = "");
    }
}
