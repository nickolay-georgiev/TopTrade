namespace TopTrade.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.Administrator;
    using TopTrade.Web.ViewModels.Administration;

    public class UsersController : BaseAdministrationController
    {
        private const int ItemsPerPage = 20;
        private readonly IAdministrationService administrationService;

        public UsersController(
            IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            var allUsersPageViewModel = await this.administrationService.GetAllUsersAsync(id, ItemsPerPage);

            return this.View(allUsersPageViewModel);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var userViewModel = this.administrationService.GetUserById<UserInUsersPageViewModel>(id);

            if (userViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(userViewModel);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var userViewModel = this.administrationService.GetUserById<AccountManagerViewModel>(id);

            if (userViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateManager(CreateManagerInputModel input)
        {
            if (input.Id == null)
            {
                return this.NotFound();
            }

            try
            {
                await this.administrationService.CreateManager(input);
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;

                return this.RedirectToAction(nameof(this.All));
            }

            this.TempData["Success"] = "Success!";
            this.TempData["Message"] = "Manger successfuly created";

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
