namespace TopTrade.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.Administrator;
    using TopTrade.Web.ViewModels.Administration;

    public class AccountManagersController : BaseAdministrationController
    {
        private const int ItemsPerPage = 20;
        private readonly IAdministrationService administrationService;

        public AccountManagersController(
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

            var allManagersPageViewModel = await this.administrationService.GetAllAccountManagersAsync(id, ItemsPerPage);

            return this.View(allManagersPageViewModel);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var userViewModel = this.administrationService.GetUserById(id);

            if (userViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditAccountManagerInputModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.administrationService.DeactivateManagerAccountAsync(id, input);

                    this.TempData["Success"] = "Success!";
                    this.TempData["Message"] = "User account was updated";

                    return this.RedirectToAction(nameof(this.All));
                }
                catch (Exception error)
                {
                    this.TempData["Error"] = "Error!";
                    this.TempData["Message"] = error.Message;

                    return this.RedirectToAction(nameof(this.Edit));
                }
            }

            return this.BadRequest();
        }

        public IActionResult SearchById(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var manager = this.administrationService.GetUserById(id);

            if (manager == null)
            {
                return this.NotFound();
            }

            var viewModel = new AccountManagerPageViewModel
            {
                Managers = new List<AccountManagerViewModel> { manager },
                PageNumber = 1,
                ItemsPerPage = ItemsPerPage,
                DataCount = 1,
            };

            return this.View(nameof(this.All), viewModel);
        }
    }
}
