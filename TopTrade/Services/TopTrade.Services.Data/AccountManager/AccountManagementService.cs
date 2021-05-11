namespace TopTrade.Services.Data.AccountManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.AccountManager;

    public class AccountManagementService : IAccountManagementService
    {
        private readonly IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository;

        public AccountManagementService(IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository)
        {
            this.verificationDocumentRepository = verificationDocumentRepository;
        }

        public ICollection<VerificationDocumentViewModel> GetAllUnverifiedUsers()
        {
            var viewModels = this.verificationDocumentRepository
                .AllAsNoTracking()
                .Where(x => x.VerificationStatus == VerificationDocumentStatus.Pending.ToString())
                .To<VerificationDocumentViewModel>()
                .ToList();

            // TODO Remove when upload on azure
            foreach (var model in viewModels)
            {
                model.DocumentUrl = model.DocumentUrl.Split("wwwroot")[1];
            }

            return viewModels;
        }

        public VerificationDocumentViewModel GetById(string id)
        {
            var document = this.verificationDocumentRepository
                .AllAsNoTracking()
                .To<VerificationDocumentViewModel>()
                .FirstOrDefault(x => x.Id == id);

            document.DocumentUrl = document.DocumentUrl.Split("wwwroot")[1];

            return document;
        }

        public async Task UpdateUserVerificationStatusAsync(string id, VerificationDocumentInputModel input)
        {
            var document = this.verificationDocumentRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (document == null)
            {
                throw new ArgumentNullException("Document not found");
            }

            document.VerificationStatus = Enum.Parse(typeof(VerificationDocumentStatus), input.VerificationStatus).ToString();

            this.verificationDocumentRepository.Update(document);
            await this.verificationDocumentRepository.SaveChangesAsync();
        }
    }
}
