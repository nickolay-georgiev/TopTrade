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
        private readonly IUserProfileService editProfileService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUploadDocumentsService uploadDocumentsService;

        public ProfileController(
            IWebHostEnvironment environment,
            IUserProfileService editProfileService,
            UserManager<ApplicationUser> userManager,
            IUploadDocumentsService uploadDocumentsService)
        {
            this.environment = environment;
            this.userManager = userManager;
            this.editProfileService = editProfileService;
            this.uploadDocumentsService = uploadDocumentsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var userDto = this.editProfileService.GetUserDataProfilePage(user);

            // TODO Remove this when upload to azure
            // var test = userDto.AvatarUrl == null ? "/img/face-3.jpg" : userDto.AvatarUrl.Split("wwwroot")[1];
            var test = userDto.AvatarUrl;

            var userInputModel = new EditProfileViewModel
            {
                FirstName = userDto.FirstName,
                MiddleName = userDto.MiddleName,
                LastName = userDto.LastName,
                Address = userDto.Address,
                City = userDto.City,
                Country = userDto.Country,
                ZipCode = userDto.ZipCode,
                AboutMe = userDto.AboutMe,
                // AvatarUrl = user.AvatarUrl,
                AvatarUrl = test,
            };

            return this.View(userInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                this.TempData["Error"] = "Error!";
                this.TempData["Message"] = "Invalid Data!";
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                await this.editProfileService.EditProfileAsync(user, input);
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
        public async Task<IActionResult> UploadAvatar(EditProfileViewModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.uploadDocumentsService.UploadDocumentsAsync(input.UserAvatarInputModel, userId, $"{this.environment.WebRootPath}/users/profile-images/{userId}");
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
