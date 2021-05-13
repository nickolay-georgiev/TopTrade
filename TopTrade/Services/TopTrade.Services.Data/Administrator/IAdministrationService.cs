namespace TopTrade.Services.Data.Administrator
{
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.Administration;

    public interface IAdministrationService
    {
        Task<AdministratorDashboardViewModel> GetAdminDashboardDataAsync();

        Task<UsersPageViewModel> GetAllUsersAsync(int pageNumber, int itemsPerPage);

        AccountManagerViewModel GetUserById(string id);

        Task<AccountManagerPageViewModel> GetAllAccountManagersAsync(int pageNumber, int itemsPerPage);

        Task DeactivateManagerAccountAsync(string id, EditAccountManagerInputModel input);

        Task CreateManager(CreateManagerInputModel input);
    }
}
