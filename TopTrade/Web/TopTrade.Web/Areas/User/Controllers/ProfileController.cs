﻿namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Data;
    using TopTrade.Web.ViewModels.User;

    public class ProfileController : BaseLoggedUserController
    {
        private readonly IWebHostEnvironment environment;
        private readonly IUploadDocumentsService uploadDocumentsService;

        public ProfileController(
            IWebHostEnvironment environment,
            IUploadDocumentsService uploadDocumentsService)
        {
            this.environment = environment;
            this.uploadDocumentsService = uploadDocumentsService;
        }

        public IActionResult Index()
        {
            UserAvatarInputModel userAvatarInputModel = new UserAvatarInputModel();
            return this.View(userAvatarInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocuments(VerificationDocumentsInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                await this.uploadDocumentsService.UploadDocumentsAsync(input, userId, $"{this.environment.WebRootPath}/user/verification-documents/{userId}");
            }
            catch (System.Exception error)
            {
                this.ViewData["Error"] = "Error!";
                this.ViewData["Message"] = error.Message;
                return this.View(nameof(this.Index));
            }

            this.ViewData["Success"] = "Successfully uploaded!";
            this.ViewData["Message"] = "Wait for approvement";

            return this.View(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(UserAvatarInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.uploadDocumentsService.UploadAvatarAsync(input, userId, $"{this.environment.WebRootPath}/user/profile-images");
            return this.View(nameof(this.Index));
        }
    }
}
