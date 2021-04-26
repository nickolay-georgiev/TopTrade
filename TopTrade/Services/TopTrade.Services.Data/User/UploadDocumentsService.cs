namespace TopTrade.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Web.ViewModels.User;

    public class UploadDocumentsService : IUploadDocumentsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository;

        public UploadDocumentsService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository)
        {
            this.userManager = userManager;
            this.verificationDocumentRepository = verificationDocumentRepository;
        }

        public async Task UploadDocumentsAsync(VerificationDocumentsInputModel input, string userId, string imagePath)
        {
            this.VerifyDocumentsAreValid(input);

            Directory.CreateDirectory($"{imagePath}");
            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');

                var currentDocument = new VerificationDocument
                {
                    VerificationStatus = VerificationDocumentStatus.Pending.ToString(),
                    UserId = userId,
                };

                string physicalPath = $"{imagePath}/{currentDocument.Id}.{extension}";
                currentDocument.DocumentUrl = physicalPath;
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await document.CopyToAsync(fileStream);
                await this.verificationDocumentRepository.AddAsync(currentDocument);
            }

            await this.verificationDocumentRepository.SaveChangesAsync();
        }


        public async Task UploadAvatarAsync(UserAvatarInputModel input, string userId, string imagePath)
        {
            this.VerifyDocumentsAreValid(input);

            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');

                string dbImage = Guid.NewGuid().ToString();
                string physicalPath = $"{imagePath}/{dbImage}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await document.CopyToAsync(fileStream);
            }
        }

        private void VerifyDocumentsAreValid(IUploadFiles input)
        {
            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.').ToUpper();
                if (!Enum.IsDefined(typeof(AllowedFileExtensions), extension))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                if (document.Length > (5 * 1024 * 1024))
                {
                    throw new Exception("File size is bigger than 5MB.");
                }
            }
        }
    }
}
