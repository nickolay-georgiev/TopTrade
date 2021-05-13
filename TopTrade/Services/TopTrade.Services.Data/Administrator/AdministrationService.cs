namespace TopTrade.Services.Data.Administrator
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Common;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.Administration;

    public class AdministrationService : IAdministrationService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Trade> tradeRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public AdministrationService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Trade> tradeRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.tradeRepository = tradeRepository;
            this.userRepository = userRepository;
        }

        public async Task<AdministratorDashboardViewModel> GetAdminDashboardDataAsync()
        {
            var adminRole = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRoleName);
            var managerRole = await this.roleManager.FindByNameAsync(GlobalConstants.AccountManagerRoleName);

            var totalUsersThisMonth = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.CreatedOn.Month == DateTime.Now.Month && x.Roles
                .All(x => x.RoleId != managerRole.Id && x.RoleId != adminRole.Id))
                .Count();

            var totalUsersWithDeletedAccounts = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.Roles
                .All(x => x.RoleId != managerRole.Id && x.RoleId != adminRole.Id))
                .Count();

            var totalAccountManagers = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.Roles
                .Any(x => x.RoleId == managerRole.Id))
                .Count();

            var totalProfitFromFees = this.tradeRepository
                .AllAsNoTrackingWithDeleted()
                .Sum(x => x.SwapFee);

            var dashboardViewModel = new AdministratorDashboardViewModel()
            {
                TotalUserThisMonth = totalUsersThisMonth,
                TotalUsersWithDeletedAccounts = totalUsersWithDeletedAccounts,
                TotalAccountManagers = totalAccountManagers,
                TotalProfitFromFees = totalProfitFromFees,
            };

            return dashboardViewModel;
        }

        public async Task<UsersPageViewModel> GetAllUsersAsync(int pageNumber, int itemsPerPage)
        {
            var adminRole = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRoleName);
            var managerRole = await this.roleManager.FindByNameAsync(GlobalConstants.AccountManagerRoleName);

            var users = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.Roles
                .All(x => x.RoleId != managerRole.Id && x.RoleId != adminRole.Id))
                .OrderByDescending(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<UserInUsersPageViewModel>()
                .ToList();

            var pageViewModel = new UsersPageViewModel
            {
                Users = users,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.Roles
                .Any(x => x.RoleId != managerRole.Id && x.RoleId != adminRole.Id))
                .Count(),
            };

            return pageViewModel;
        }

        public AccountManagerViewModel GetUserById(string id)
        {
            return this.userRepository
                .AllAsNoTrackingWithDeleted()
                .To<AccountManagerViewModel>()
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<AccountManagerPageViewModel> GetAllAccountManagersAsync(int pageNumber, int itemsPerPage)
        {
            var managerRole = await this.roleManager.FindByNameAsync(GlobalConstants.AccountManagerRoleName);

            var managers = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.Roles
                .Any(x => x.RoleId == managerRole.Id))
                .OrderByDescending(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<AccountManagerViewModel>()
                .ToList();

            var pageViewModel = new AccountManagerPageViewModel
            {
                Managers = managers,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.userRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.Roles
                .Any(x => x.RoleId == managerRole.Id))
                .Count(),
            };

            return pageViewModel;
        }

        public async Task DeactivateManagerAccountAsync(string id, EditAccountManagerInputModel input)
        {
            var manager = this.userRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (manager == null)
            {
                throw new ArgumentNullException("Manager not found");
            }

            manager.IsDeleted = input.IsDeleted;

            this.userRepository.Update(manager);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task CreateManager(CreateManagerInputModel input)
        {
            var manager = this.userRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == input.Id);

            if (manager == null)
            {
                throw new ArgumentNullException("Manager not found");
            }

            await this.userManager.AddToRoleAsync(manager, GlobalConstants.AccountManagerRoleName);

            this.userRepository.Update(manager);
            await this.userRepository.SaveChangesAsync();
        }
    }
}
