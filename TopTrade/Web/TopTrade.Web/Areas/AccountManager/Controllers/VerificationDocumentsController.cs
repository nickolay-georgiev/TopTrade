namespace TopTrade.Web.Areas.AccountManager.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data;
    using TopTrade.Services.Data.AccountManager;
    using TopTrade.Web.ViewModels.AccountManager;

    public class VerificationDocumentsController : BaseAccountManagerController
    {
        private readonly IAccountManagementService accountManagementService;

        public VerificationDocumentsController(
            IAccountManagementService accountManagementService)
        {
            this.accountManagementService = accountManagementService;
        }

        public IActionResult All()
        {
            var verificationDocumentViewModels = this.accountManagementService.GetAllUnverifiedUsers();
            return this.View(verificationDocumentViewModels);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var verificationDocument = this.accountManagementService.GetById(id);
            if (verificationDocument == null)
            {
                return this.NotFound();
            }

            return this.View(verificationDocument);
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
                }
                catch (Exception error)
                {
                    this.TempData["Error"] = "Error!";
                    this.TempData["Message"] = error.Message;

                    return this.RedirectToAction(nameof(this.Edit));
                }
            }

            this.TempData["Success"] = "Success!";
            this.TempData["Message"] = "Account was updated";

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
