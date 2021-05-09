namespace TopTrade.Services.Data.User
{
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User.Models;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public interface IUserService
    {
        Task EditProfileAsync(ApplicationUser user, EditProfileViewModel input);

        UserProfileDto GetUserDataProfilePage(ApplicationUser user);

        UserCardDto GetUserDataCardComponent(ApplicationUser user);

        string GetUserVerificationStatus(ApplicationUser user);

        WalletHistoryViewModel GetWalletHitoryData(string userId, int pageNumber, int itemsPerPage = 8);

        Task InitializeUserCollectionsAsync(ApplicationUser user);

        Task UpdateUserAccountAsync(DepositModalInputModel input, string userId);

        WithdrawViewModel GetAvailableUserWithdrawData(string userId);

        Task AcceptWithdrawRequest(WithdrawInputModel input, string userId);
    }
}
