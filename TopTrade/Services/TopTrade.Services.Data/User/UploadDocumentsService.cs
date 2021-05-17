namespace TopTrade.Services.Data
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Common;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;

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

        public async Task UploadDocumentsAsync(BaseUploadFileInputModel input, string userId, string imagePath, [CallerMemberName]string callerName = "")
        {
            this.VerifyDocumentsAreValid(input);

            Directory.CreateDirectory($"{imagePath}");
            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');

                if (callerName.Equals(GlobalConstants.UploadVerificationDocuments))
                {
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
                    await this.verificationDocumentRepository.SaveChangesAsync();
                }
                else if (callerName.Equals(GlobalConstants.UploadAvatar))
                {
                    var user = await this.userManager.FindByIdAsync(userId);
                    string physicalPath = $"{imagePath}.{extension}";
                    user.AvatarUrl = physicalPath;

                    using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                    await document.CopyToAsync(fileStream);

                    await this.userManager.UpdateAsync(user);
                }
            }
        }

        private void VerifyDocumentsAreValid(IUploadFiles input)
        {
            if (input.Documents.Count > 3)
            {
                throw new ArgumentOutOfRangeException(GlobalConstants.ExceedMaximumDocumentsCount);
            }

            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.').ToUpper();
                if (!Enum.IsDefined(typeof(AllowedFileExtensions), extension))
                {
                    throw new InvalidOperationException(string.Format(GlobalConstants.InvalidFileExtension, extension));
                }

                if (document.Length > (5 * 1024 * 1024))
                {
                    throw new InvalidOperationException(GlobalConstants.ExceedMaximumDocumentSize);
                }
            }
        }
    }
}
