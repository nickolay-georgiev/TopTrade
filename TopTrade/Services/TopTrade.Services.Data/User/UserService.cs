namespace TopTrade.Services.Data.User
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using TopTrade.Common;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
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

        public async Task EditProfileAsync(string userId, EditProfileInputModel input)
        {
            var user = this.userRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

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

        public UserProfileViewModel GetUserDataProfilePage(string userId)
        {
            var user = this.userRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == userId);

            var userViewModel = new UserProfileViewModel
            {
                Id = user.Id,
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

            // TODO Remove this when upload to azure
            userViewModel.AvatarUrl = userViewModel.AvatarUrl == null ?
                "/img/default-user-avatar.webp" : userViewModel.AvatarUrl.Split("wwwroot")[2];

            //userViewModel.AvatarUrl = userViewModel.AvatarUrl == null ?
            //  "/img/default-user-avatar.webp" : userViewModel.AvatarUrl;

            return userViewModel;
        }

        public UserProfileCardViewModel GetUserDataCardComponent(ApplicationUser user)
        {
            var userCardDto = this.userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == user.Id)
                .To<UserProfileCardViewModel>()
                .FirstOrDefault();

            userCardDto.AvatarUrl = userCardDto.AvatarUrl == null ?
                "/img/default-user-avatar.webp" : user.AvatarUrl.Split("wwwroot")[2];

            userCardDto.Username = user.Email.Split("@")[0];

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
                ? GlobalConstants.DisplayedStatusVerified : GlobalConstants.DisplayedStatusNotVerified;

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

                await this.cardRepository.AddAsync(card);
                await this.cardRepository.SaveChangesAsync();
            }

            await this.depositRepository
                .AddAsync(new Deposit
                {
                    Amount = input.Amount,
                    Currency = AcceptedCurrencies.USD.ToString(),
                    PaymentMethod = GlobalConstants.PaymentMethod,
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

        public async Task AcceptWithdrawRequestAsync(WithdrawInputModel input, string userId)
        {

            if (input.Available < input.DesiredAmount)
            {
                throw new InvalidOperationException(string.Format(GlobalConstants.ReachedWithdrawLimit, input.Available));
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
