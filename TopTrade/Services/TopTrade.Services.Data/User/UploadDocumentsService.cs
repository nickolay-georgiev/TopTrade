namespace TopTrade.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.User;

    public class UploadDocumentsService : IUploadDocumentsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "tif", "pdf" };

        public async Task UploadDocumentsAsync(VerificationDocumentsInputModel input, string imagePath)
        {
            this.VerifyDocumentsAreValid(input, imagePath);

            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');

                //var dbImage = new Imager
                //{
                //    AddedByUserId = userId,
                //    Extension = extension,
                //};
                //recipe.Images.Add(dbImage);

                string dbImage = Guid.NewGuid().ToString();
                string physicalPath = $"{imagePath}/{dbImage}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await document.CopyToAsync(fileStream);
            }
        }


        public async Task UploadAvatarAsync(UserAvatarInputModel input, string imagePath)
        {
            this.VerifyDocumentsAreValid(input, imagePath);

            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');

                string dbImage = Guid.NewGuid().ToString();
                string physicalPath = $"{imagePath}/{dbImage}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await document.CopyToAsync(fileStream);
            }
        }

        private void VerifyDocumentsAreValid(IUploadFiles input, string imagePath)
        {
            Directory.CreateDirectory($"{imagePath}");
            foreach (var document in input.Documents)
            {
                var extension = Path.GetExtension(document.FileName).TrimStart('.');
                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
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
