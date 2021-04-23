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
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public async Task UploadDocumentsAsync(VerificationDocumentsInputModel input, string imagePath)
        {
            // /wwwroot/images/recipes/jhdsi-343g3h453-=g34g.jpg
            Directory.CreateDirectory($"{imagePath}");
            foreach (var image in input.Documents)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                //var dbImage = new Image
                //{
                //    AddedByUserId = userId,
                //    Extension = extension,
                //};
                //recipe.Images.Add(dbImage);

                string dbImage = Guid.NewGuid().ToString();

                //var physicalPath = $"{imagePath}/recipes/{dbImage.Id}.{extension}";
                var physicalPath = $"{imagePath}/{dbImage}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }
        }
    }
}
