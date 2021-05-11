namespace TopTrade.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TopTrade.Common;
    using TopTrade.Data.Models;

    public class AccountManagerSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Any(x => x.Email == configuration["AccountManager:Email"]))
            {
                return;
            }

            var accountManager = new ApplicationUser
            {
                UserName = configuration["AccountManager:UserName"],
                Email = configuration["AccountManager:Email"],
                EmailConfirmed = true,
            };

            var accountManagerPassword = configuration["AccountManager:Password"];

            var result = await userManager.CreateAsync(accountManager, accountManagerPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(accountManager, GlobalConstants.AccountManagerRoleName);
            }
        }
    }
}
