namespace TopTrade.Services.Data.User
{
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public interface IUserService
    {
        Task EditProfileAsync(string userId, EditProfileInputModel input);

        UserProfileViewModel GetUserDataProfilePage(string userId);

        UserProfileCardViewModel GetUserDataCardComponent(ApplicationUser user);

        string GetUserVerificationStatus(ApplicationUser user);

        WalletHistoryViewModel GetWalletHitoryData(string userId, int pageNumber, int itemsPerPage = 8);

        Task InitializeUserCollectionsAsync(ApplicationUser user);

        Task UpdateUserAccountAsync(DepositModalInputModel input, string userId);

        WithdrawViewModel GetAvailableUserWithdrawData(string userId);

        Task AcceptWithdrawRequestAsync(WithdrawInputModel input, string userId);
    }
}
