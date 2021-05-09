namespace TopTrade.Services.Data.User
{
    using System;
    using System.Linq;
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
    using TopTrade.Web.ViewModels.User.ViewComponents;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<Card> cardRepository;
        private readonly IDeletableEntityRepository<Deposit> depositRepository;
        private readonly IDeletableEntityRepository<Withdraw> withdrawRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatisticRepository;
        private readonly IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository;

        public UserService(
            IDeletableEntityRepository<Card> cardRepository,
            IDeletableEntityRepository<Deposit> depositRepository,
            IDeletableEntityRepository<Withdraw> withdrawRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<AccountStatistic> accountStatisticRepository,
            IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository)
        {
            this.cardRepository = cardRepository;
            this.userRepository = userRepository;
            this.depositRepository = depositRepository;
            this.withdrawRepository = withdrawRepository;
            this.accountStatisticRepository = accountStatisticRepository;
            this.verificationDocumentRepository = verificationDocumentRepository;
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

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
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
                // AvatarUrl = user.AvatarUrl,
                AvatarUrl = test,
                Username = user.Email.Split("@")[0],
                VerificationStatus = verificationStatus,
            };

            return userCardDto;
        }

        public async Task InitializeUserCollectionsAsync(ApplicationUser user)
        {
            var initialStatistic = new AccountStatistic { UserId = user.Id };
            var initialWatchlist = new Watchlist { UserId = user.Id };

            user.AccountStatistic = initialStatistic;
            user.Watchlist = initialWatchlist;

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
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
                    DataCount = this.depositRepository.AllAsNoTracking()
                    .Where(x => x.UserId == userId).Count(),
                },
                WithdrawPaging = new PagingViewModel
                {
                    ItemsPerPage = itemsPerPage,
                    PageNumber = pageNumber,
                    DataCount = this.withdrawRepository.AllAsNoTracking()
                    .Where(x => x.UserId == userId).Count(),
                },
                Deposits = depositsViewModel,
                Withdraws = withdrawsViewModel,
            };

            return walletHistoryViewModel;
        }

        public async Task UpdateUserAccountAsync(DepositModalInputModel input, string userId)
        {
            var currentCardNumber = input.Card.Number;

            currentCardNumber =
                currentCardNumber.Substring(0, 4) + '-' + new string('*', 4) + '-' + new string('*', 4) +
                currentCardNumber.Substring(currentCardNumber.Length - 4, 4);

            var card = this.cardRepository
                .All()
                .Where(x => x.UserId == userId && x.Number == currentCardNumber)
                .FirstOrDefault();

            if (card == null)
            {
                card = new Card
                {
                    Number = currentCardNumber,
                    UserId = userId,
                };

                _ = this.cardRepository.AddAsync(card);
                await this.cardRepository.SaveChangesAsync();
            }

            _ = this.depositRepository
                .AddAsync(new Deposit
                {
                    Amount = input.Amount,
                    Currency = "USD",
                    PaymentMethod = "Credit/Debit Card",
                    UserId = userId,
                    CardId = card.Id,
                    TransactionStatus = TransactionStatus.Completed.ToString(),
                });

            await this.depositRepository.SaveChangesAsync();

            var currentUserAccoutStatistic = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();

            currentUserAccoutStatistic.Equity += input.Amount;
            currentUserAccoutStatistic.Available += input.Amount;

            this.accountStatisticRepository.Update(currentUserAccoutStatistic);
            await this.accountStatisticRepository.SaveChangesAsync();
        }

        public WithdrawViewModel GetAvailableUserWithdrawData(string userId)
        {
            var withdrawViewModel = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<WithdrawViewModel>()
                .FirstOrDefault();

            return withdrawViewModel;
        }

        public async Task AcceptWithdrawRequest(WithdrawInputModel input, string userId)
        {
            if (input.Available < input.DesiredAmount)
            {
                throw new InvalidOperationException($"You can withdraw up to ${input.Available}");
            }

            var card = this.cardRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && x.Number == input.Card)
                .FirstOrDefault();

            var withdraw = new Withdraw
            {
                Amount = input.DesiredAmount,
                UserId = userId,
                CardId = card.Id,
                TransactionStatus = TransactionStatus.Pending.ToString(),
            };

            await this.withdrawRepository.AddAsync(withdraw);
            await this.withdrawRepository.SaveChangesAsync();
        }
    }
}
