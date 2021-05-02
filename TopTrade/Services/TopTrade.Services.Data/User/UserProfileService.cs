namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Data.User.Models;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;

    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository;
        private readonly IDeletableEntityRepository<Deposit> depositRepository;
        private readonly IDeletableEntityRepository<Withdraw> withdrawRepository;

        public UserProfileService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository,
            IDeletableEntityRepository<Deposit> depositRepository,
            IDeletableEntityRepository<Withdraw> withdrawRepository)
        {
            this.userManager = userManager;
            this.verificationDocumentRepository = verificationDocumentRepository;
            this.depositRepository = depositRepository;
            this.withdrawRepository = withdrawRepository;
        }

        public async Task EditProfileAsync(ApplicationUser user, EditProfileViewModel input)
        {
            user.FirstName = input.FirstName;
            user.MiddleName = input.MiddleName;
            user.LastName = input.LastName;
            user.Address = input.Address;
            user.City = input.City;
            user.Country = input.Country;
            user.ZipCode = input.ZipCode;
            user.AboutMe = input.AboutMe;

            await this.userManager.UpdateAsync(user);
        }

        public UserProfileDto GetUserDataProfilePage(ApplicationUser user)
        {
            var userDto = new UserProfileDto
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                ZipCode = user.ZipCode,
                AboutMe = user.AboutMe,
                AvatarUrl = user.AvatarUrl,
            };

            return userDto;
        }

        public UserCardDto GetUserDataCardComponent(ApplicationUser user)
        {
            string verificationStatus = this.GetUserVerificationStatus(user);

            // TODO Remove this when upload to azure
            var test = user.AvatarUrl == null ? "/img/default-user-avatar.webp" : user.AvatarUrl.Split("wwwroot")[1];

            var userCardDto = new UserCardDto
            {
                //AvatarUrl = user.AvatarUrl,
                AvatarUrl = test,
                Username = user.Email.Split("@")[0],
                VerificationStatus = verificationStatus,
            };

            return userCardDto;
        }

        public string GetUserVerificationStatus(ApplicationUser user)
        {
            var documents = this.verificationDocumentRepository
                            .AllAsNoTracking()
                            .Where(d => d.UserId == user.Id)
                            .ToList();

            var verificationStatus =
                documents.Any() &&
                documents.All(d => d.VerificationStatus == VerificationDocumentStatus.Approved.ToString())
                ? "Verified" : "Not Verified";

            return verificationStatus;
        }

        public WalletHistoryViewModel GetWalletHitoryData(string userId, int pageNumber, int itemsPerPage = 8)
        {
            var depositsViewModel = this.depositRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<DepositInWalletHistoryViewModel>()
                .ToList();

            var withdrawsViewModel = this.withdrawRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.CreatedOn)
               .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
               .To<WithdrawInWalletHistoryViewModel>()
               .ToList();

            var walletHistoryViewModel = new WalletHistoryViewModel
            {
                DepositsPaging = new PagingViewModel
                {
                    ItemsPerPage = itemsPerPage,
                    PageNumber = pageNumber,
                    DataCount = this.depositRepository.AllAsNoTracking().Where(x => x.UserId == userId).Count(),
                },
                Deposits = depositsViewModel,
                WithdrawPaging = new PagingViewModel
                {
                    ItemsPerPage = itemsPerPage,
                    PageNumber = pageNumber,
                    DataCount = this.withdrawRepository.AllAsNoTracking().Where(x => x.UserId == userId).Count(),
                },
                Withdraws = withdrawsViewModel,
            };

            return walletHistoryViewModel;
        }
    }
}
