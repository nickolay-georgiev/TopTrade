namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data;
    using TopTrade.Services.Data.User;
    using TopTrade.Web.ViewModels.User;

    public class ProfileController : BaseLoggedUserController
    {
        private readonly IWebHostEnvironment environment;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUploadDocumentsService uploadDocumentsService;

        public ProfileController(
            IWebHostEnvironment environment,
            IUserService editProfileService,
            UserManager<ApplicationUser> userManager,
            IUploadDocumentsService uploadDocumentsService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.userService = editProfileService;
            this.uploadDocumentsService = uploadDocumentsService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userProfileViewModel = this.userService.GetUserDataProfilePage(userId);

            return this.View(userProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId != input.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = "Invalid Data!";
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                await this.userService.EditProfileAsync(userId, input);
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData["Success"] = "Success!";
            this.TempData["Message"] = "Your data was updated!";

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocuments(VerificationDocumentsInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.uploadDocumentsService.UploadDocumentsAsync(input, userId, $"{this.environment.WebRootPath}/users/verification-documents/{userId}");
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData["Success"] = "Successfully uploaded!";
            this.TempData["Message"] = "Wait for approvement";

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(UserAvatarInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.uploadDocumentsService.UploadDocumentsAsync(input, userId, $"{this.environment.WebRootPath}/users/profile-images/{userId}");
            }
            catch (Exception error)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = error.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData["Success"] = "Successfully uploaded!";
            this.TempData["Message"] = "Enjoy your new avatar";

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
