namespace TopTrade.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
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

        public async Task UploadDocumentsAsync(IUploadFiles input, string userId, string imagePath, [CallerMemberName]string callerName = "")
        {
            this.VerifyDocumentsAreValid(input);

            Directory.CreateDirectory($"{imagePath}");
            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');

                if (callerName.Equals("UploadDocuments"))
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
                else if (callerName.Equals("UploadAvatar"))
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
                throw new Exception("You can not upload more than 3 documents");
            }

            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.').ToUpper();
                if (!Enum.IsDefined(typeof(AllowedFileExtensions), extension))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                if (document.Length > (5 * 1024 * 1024))
                {
                    throw new Exception("File size is bigger than 5MB");
                }
            }
        }
    }
}
