﻿namespace TopTrade.Web.Areas.User.Controllers
{
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
            await this.uploadDocumentsService.UploadDocumentsAsync(input, $"{this.environment.WebRootPath}/user/verification-documents");
            return this.RedirectToAction(nameof(this.Index), "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(UserAvatarInputModel input)
        {
            await this.uploadDocumentsService.UploadAvatarAsync(input, $"{this.environment.WebRootPath}/user/profile-images");
            return this.View(nameof(this.Index));
        }
    }
}