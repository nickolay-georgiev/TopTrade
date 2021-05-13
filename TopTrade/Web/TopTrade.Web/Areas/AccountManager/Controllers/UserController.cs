namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.AccountManager;
    using TopTrade.Web.ViewModels.AccountManager;

    public class UserController : BaseAccountManagerController
    {
        private const int ItemsPerPage = 20;
        private readonly IAccountManagementService accountManagementService;

        public UserController(
            IAccountManagementService accountManagementService)
        {
            this.accountManagementService = accountManagementService;
        }

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            var allUsersPageViewModel = await this.accountManagementService.GetAllUsersAsync(id, ItemsPerPage);

            return this.View(allUsersPageViewModel);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var userViewModel = this.accountManagementService.GetUserById<UserInPageViewModel>(id);

            if (userViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserInputModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.accountManagementService.DeactivateUserAccountAsync(id, input);

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

            var user = this.accountManagementService.GetUserById<UserInPageViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            var viewModel = new UsersPageViewModel
            {
                Users = new List<UserInPageViewModel> { user },
                PageNumber = 1,
                ItemsPerPage = ItemsPerPage,
                DataCount = 1,
            };

            return this.View(nameof(this.All), viewModel);
        }
    }
}
