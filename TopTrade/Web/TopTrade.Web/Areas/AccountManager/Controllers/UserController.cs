namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using System;
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

        public IActionResult All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            var allUsersPageViewModel = this.accountManagementService.GetAllUsers(id, ItemsPerPage);
            return this.View(allUsersPageViewModel);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var userViewModel = this.accountManagementService.GetUserById(id);

            if (userViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserInputModel input)
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
    }
}
