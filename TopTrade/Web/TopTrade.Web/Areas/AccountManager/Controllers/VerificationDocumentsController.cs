namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data.AccountManager;
    using TopTrade.Web.ViewModels.AccountManager;

    public class VerificationDocumentsController : BaseAccountManagerController
    {
        private const int ItemsPerPage = 20;
        private readonly IAccountManagementService accountManagementService;

        public VerificationDocumentsController(
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

            var verificationDocumentViewModels = this.accountManagementService.GetAllUnverifiedUsers(id, ItemsPerPage);
            return this.View(verificationDocumentViewModels);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            try
            {
                var verificationDocument = this.accountManagementService.GetUnverifiedUserById(id);
                return this.View(verificationDocument);
            }
            catch (Exception)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, VerificationDocumentInputModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.accountManagementService.UpdateUserVerificationStatusAsync(id, input);

                    this.TempData["Success"] = "Success!";
                    this.TempData["Message"] = "Account was updated";

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
