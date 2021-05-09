namespace TopTrade.Services.Data
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.User;

    public interface IUploadDocumentsService
    {
        Task UploadDocumentsAsync(IUploadFiles input, string userId, string imagePath, [CallerMemberName] string callerName = "");
    }
}
