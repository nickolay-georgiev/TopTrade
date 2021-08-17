namespace TopTrade.Services.Data.Tests.ServiceTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;

    public static class TestDataHelpers
    {
        //public static List<Account> GetTestData()
        //{
        //    var users = GetTestUsers();
        //    //var positions = GetTestPositions();
        //    //var fees = GetTestFeePayments();

        //    return new List<ApplicationUser>()
        //    {
        //        new Account
        //        {
        //            Id = 1,
        //            UserId = "1",
        //            User = users[0],
        //            Balance = 2000M,
        //            MonthlyFee = 50,
        //            Positions = positions.Where(p => p.AccountId == 1).ToList(),
        //            Fees = fees.Where(f => f.AccountId == 1).ToList(),
        //        },
        //        new Account
        //        {
        //            Id = 2,
        //            UserId = "2",
        //            User = users[1],
        //            Balance = 5000M,
        //            MonthlyFee = 50,
        //            Positions = positions.Where(p => p.AccountId == 2).ToList(),
        //            Fees = fees.Where(f => f.AccountId == 2).ToList(),
        //        },
        //    };
        //}

        public static List<ApplicationUser> GetTestUser()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    CreatedOn = DateTime.Now,
                    FirstName = "Ivan",
                    LastName = "Davidov",
                    UserName = "testUser",
                    Email = "testUser@test.com",
                    AvatarUrl = null,
                    Documents = new List<VerificationDocument>
                    {
                        new VerificationDocument
                        {
                            Id = "1",
                        },
                    },
                },
            };
        }

        public static List<UserProfileCardViewModel> GetUserProfileCardViewModel()
        {
            return new List<UserProfileCardViewModel>
            {
                new UserProfileCardViewModel
                {
                    Username = "Ivan",
                    AvatarUrl = null,
                    VerificationStatus = "Approved",
                },
            };
        }

        public static VerificationDocument GetTestUserVerificationDocument()
        {
            return new VerificationDocument
            {
                Id = "1",
                CreatedOn = DateTime.Now,
                DocumentUrl = "testVerificationDocumentUrl",
                VerificationStatus = "Approved",
            };
        }

        public static EditProfileInputModel GetTestData()
        {
            return new EditProfileInputModel()
            {
                FirstName = "A",
                MiddleName = "M",
                LastName = "l",
                Address = "sofia str.",
                City = "sofia",
                Country = "bulgaria",
                ZipCode = "8500",
                AboutMe = null,
            };
        }
    }
}
