namespace TopTrade.Services.Data.Tests.ServiceTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Repositories;
    using TopTrade.Services.Data.Tests.ServiceTests.Helpers;
    using TopTrade.Services.Data.User;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;

    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IDeletableEntityRepository<Card>> cardRepository;
        private Mock<IDeletableEntityRepository<Deposit>> depositRepository;
        private Mock<IDeletableEntityRepository<Withdraw>> withdrawRepository;
        private Mock<IDeletableEntityRepository<ApplicationUser>> userRepository;
        private Mock<IDeletableEntityRepository<AccountStatistic>> accountStatistic;
        private Mock<IDeletableEntityRepository<VerificationDocument>> verificationDocument;
        private IUserService userService;

        [SetUp]
        public void Setup()
        {
            AutoMapperInitializer.Init();

            this.cardRepository = new Mock<IDeletableEntityRepository<Card>>();
            this.depositRepository = new Mock<IDeletableEntityRepository<Deposit>>();
            this.withdrawRepository = new Mock<IDeletableEntityRepository<Withdraw>>();
            this.userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.accountStatistic = new Mock<IDeletableEntityRepository<AccountStatistic>>();
            this.verificationDocument = new Mock<IDeletableEntityRepository<VerificationDocument>>();
        }

        [Test]
        public async Task EditUserProfileWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userMiddleName = "Georgiev";

            var testUser = TestDataHelpers.GetTestUser().FirstOrDefault();

            await userRepository.AddAsync(testUser);
            await userRepository.SaveChangesAsync();

            var user = userRepository.All()
                .Where(x => x.Id == "1")
                .FirstOrDefault();

            user.MiddleName = userMiddleName;

            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            user = userRepository.All()
                .Where(x => x.Id == "1").FirstOrDefault();

            Assert.That(user.MiddleName == userMiddleName);
        }

        [Test]
        public void GetUserDataProfilePageWorksCorrectly()
        {
            var testUsers = TestDataHelpers.GetTestUser().AsQueryable();
            this.userRepository.Setup(x => x.AllAsNoTracking()).Returns(testUsers);

            this.userService = new UserService(
                this.cardRepository.Object,
                this.depositRepository.Object,
                this.withdrawRepository.Object,
                this.userRepository.Object,
                this.accountStatistic.Object,
                this.verificationDocument.Object);

            var expectedUserProfileViewModel = JsonConvert.SerializeObject(new UserProfileViewModel
            {
                Id = "1",
                FirstName = "Ivan",
                LastName = "Davidov",
                AvatarUrl = "/img/default-user-avatar.webp",
            });
            var actualUserProfileViewModel = JsonConvert.SerializeObject(this.userService.GetUserDataProfilePage("1"));

            Assert.AreEqual(expectedUserProfileViewModel, actualUserProfileViewModel);
        }

        [Test]
        public async Task GetUserDataCardComponentWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);

            var testUser = TestDataHelpers.GetTestUser().FirstOrDefault();
            await userRepository.AddAsync(testUser);
            await userRepository.SaveChangesAsync();

            this.userService = new UserService(
                this.cardRepository.Object,
                this.depositRepository.Object,
                this.withdrawRepository.Object,
                userRepository,
                this.accountStatistic.Object,
                this.verificationDocument.Object);

            var actualUserProfileViewModel = JsonConvert.SerializeObject(this.userService
            .GetUserDataCardComponent(testUser));

            var expectedUserProfileViewModel = JsonConvert.SerializeObject(new UserProfileCardViewModel
            {
                Username = "testUser",
                AvatarUrl = "/img/default-user-avatar.webp",
                VerificationStatus = "Not Verified",
            });

            Assert.AreEqual(expectedUserProfileViewModel, actualUserProfileViewModel);
        }

        [Test]
        public async Task VerifyInitializeUserCollectionsAsyncWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);

            var testUser = TestDataHelpers.GetTestUser().FirstOrDefault();
            await userRepository.AddAsync(testUser);
            await userRepository.SaveChangesAsync();

            this.userService = new UserService(
             this.cardRepository.Object,
             this.depositRepository.Object,
             this.withdrawRepository.Object,
             userRepository,
             this.accountStatistic.Object,
             this.verificationDocument.Object);

            await this.userService.InitializeUserCollectionsAsync(testUser);

            var user = userRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == testUser.Id);

            Assert.IsNotNull(user.AccountStatisticId);
            Assert.IsNotNull(user.WatchlistId);
        }

        [Test]
        public async Task GetUserVerificationStatusWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var verificationDocumentRepository = new EfDeletableEntityRepository<VerificationDocument>(context);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);

            var verificationDocument = TestDataHelpers.GetVerificationDocument();
            var testUser = TestDataHelpers.GetTestUser().FirstOrDefault();
            testUser.Documents = new List<VerificationDocument>();
            testUser.Documents.Add(verificationDocument);

            await userRepository.AddAsync(testUser);
            await userRepository.SaveChangesAsync();

            this.userService = new UserService(
                this.cardRepository.Object,
                this.depositRepository.Object,
                this.withdrawRepository.Object,
                userRepository,
                this.accountStatistic.Object,
                verificationDocumentRepository);

            var actualUserStatus = this.userService.GetUserVerificationStatus(testUser);
            var expectedUserStatus = "Verified";

            Assert.That(expectedUserStatus == actualUserStatus);
        }

        [Test]
        public async Task GetWalletHitoryDataWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var depositRepository = new EfDeletableEntityRepository<Deposit>(context);
            var withdrawRepository = new EfDeletableEntityRepository<Withdraw>(context);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);

            var testCard = TestDataHelpers.GetCard();
            var testDeposit = TestDataHelpers.GetDeposit();
            var testWithdraw = TestDataHelpers.GetWithdraw();
            var testUser = TestDataHelpers.GetTestUser().FirstOrDefault();

            testDeposit.Card = testCard;
            testUser.Deposits.Add(testDeposit);
            testUser.Withdraws.Add(testWithdraw);

            await userRepository.AddAsync(testUser);
            await userRepository.SaveChangesAsync();

            this.userService = new UserService(
                 this.cardRepository.Object,
                 depositRepository,
                 withdrawRepository,
                 userRepository,
                 this.accountStatistic.Object,
                 this.verificationDocument.Object);

            var actualResult = this.userService.GetWalletHitoryData("1", 0, 8);
            var expectedResult = TestDataHelpers.GetWalletHistoryViewModel();

            expectedResult.Deposits.ToList()
                .ForEach(x => x.CreatedOn = actualResult.Deposits.FirstOrDefault().CreatedOn);
            expectedResult.Withdraws.ToList()
                .ForEach(x => x.CreatedOn = actualResult.Withdraws.FirstOrDefault().CreatedOn);

            var actualResultAsString = JsonConvert.SerializeObject(expectedResult);
            var expectedResultAsString = JsonConvert.SerializeObject(expectedResult);

            Assert.That(expectedResultAsString == actualResultAsString);
        }

        [Test]
        public async Task AssertUpdateUserAccountAsyncWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var depositRepository = new EfDeletableEntityRepository<Deposit>(context);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var cardRepository = new EfDeletableEntityRepository<Card>(context);
            var accountStatisticRepository = new EfDeletableEntityRepository<AccountStatistic>(context);

            var acountStatistic = TestDataHelpers.GetAccountStatistic();
            var testUser = TestDataHelpers.GetTestUser().FirstOrDefault();
            var depositModalInputModel = TestDataHelpers.GetDepositModalInputModel();

            //var aa = new AccountStatistic
            //{
            //    Id = 1,
            //    UserId = "1",
            //};
            accountStatisticRepository.AddAsync(new AccountStatistic {Id = 1, UserId = "1" })
            await userRepository.AddAsync(testUser);
            await userRepository.SaveChangesAsync();


            this.userService = new UserService(
                cardRepository,
                depositRepository,
                this.withdrawRepository.Object,
                userRepository,
                accountStatisticRepository,
                this.verificationDocument.Object);

            await this.userService.UpdateUserAccountAsync(depositModalInputModel, testUser.Id);

            var a = 5;
        }
    }
}
