namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data;
    using TopTrade.Services.Data.AccountManager;
    using TopTrade.Web.ViewModels.AccountManager;

    public class WithdrawRequestsController : BaseAccountManagerController
    {
        private const int ItemsPerPage = 20;
        private readonly IAccountManagementService accountManagementService;

        public WithdrawRequestsController(
            IAccountManagementService accountManagementService,
            ApplicationDbContext context)
        {
            this.accountManagementService = accountManagementService;
        }

        public IActionResult All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            var pageViewModel = this.accountManagementService.GetAllWithdrawRequests(id, ItemsPerPage);
            return this.View(pageViewModel);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var withdrawViewModel = this.accountManagementService.GetWithdrawRequestById(id);

            if (withdrawViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(withdrawViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WithdrawRequestInputModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.accountManagementService.UpdateUserWithdrawRequestAsync(id, input);

                    this.TempData["Success"] = "Success!";
                    this.TempData["Message"] = "Request was updated";

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
