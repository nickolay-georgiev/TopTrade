namespace TopTrade.Services.Data.AccountManager
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.AccountManager;

    public interface IAccountManagementService
    {
        VerificationDocumentPageViewModel GetAllUnverifiedUsers(int pageNumber, int itemsPerPage);

        VerificationDocumentViewModel GetUnverifiedUserById(string id);

        Task UpdateUserVerificationStatusAsync(string id, VerificationDocumentInputModel input);

        WithdrawRequestPageViewModel GetAllWithdrawRequests(int pageNumber, int itemsPerPage);

        WithdrawRequestViewModel GetWithdrawRequestById(int? id);

        Task UpdateUserWithdrawRequestAsync(int id, WithdrawRequestInputModel input);

        Task<UsersPageViewModel> GetAllUsersAsync(int pageNumber, int itemsPerPage);

        UserInPageViewModel GetUserById(string id);

        Task DeactivateUserAccountAsync(string id, EditUserInputModel input);

        Task<DashboardViewModel> GetManagerDashboardDataAsync();
    }
}
