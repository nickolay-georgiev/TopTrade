namespace TopTrade.Web.Areas.User.Controllers
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
            return this.View();
        }

        public async Task<IActionResult> UploadDocuments(VerificationDocumentsInputModel input)
        {
            // /wwwroot/images/recipes/jhdsi-343g3h453-=g34g.jpg
            await this.uploadDocumentsService.UploadDocumentsAsync(input, $"{this.environment.WebRootPath}/verification-documents");
            return this.RedirectToAction(nameof(this.Index), "Home");
        }
    }
}
