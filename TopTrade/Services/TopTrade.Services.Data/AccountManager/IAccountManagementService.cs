namespace TopTrade.Services.Data.AccountManager
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.AccountManager;

    public interface IAccountManagementService
    {
        ICollection<VerificationDocumentViewModel> GetAllUnverifiedUsers();

        VerificationDocumentViewModel GetById(string id);

        Task UpdateUserVerificationStatusAsync(string id, VerificationDocumentInputModel input);
    }
}
