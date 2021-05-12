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
        private readonly IDeletableEntityRepository<Withdraw> withdrawRepository;
        private readonly IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository;

        public AccountManagementService(
            IDeletableEntityRepository<Withdraw> withdrawRepository,
            IDeletableEntityRepository<VerificationDocument> verificationDocumentRepository)
        {
            this.verificationDocumentRepository = verificationDocumentRepository;
            this.withdrawRepository = withdrawRepository;
        }

        public VerificationDocumentPageViewModel GetAllUnverifiedUsers(int pageNumber, int itemsPerPage)
        {
            var viewModels = this.verificationDocumentRepository
                .AllAsNoTracking()
                .Where(x => x.VerificationStatus == VerificationDocumentStatus.Pending.ToString())
                .OrderBy(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<VerificationDocumentViewModel>()
                .ToList();

            // TODO Remove when upload on azure
            foreach (var model in viewModels)
            {
                model.DocumentUrl = model.DocumentUrl.Split("wwwroot")[1];
            }

            var verificationDocumentPageViewModel = new VerificationDocumentPageViewModel
            {
                VerificationDocuments = viewModels,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.verificationDocumentRepository
                .AllAsNoTracking()
                .Where(x => x.VerificationStatus == VerificationDocumentStatus.Pending.ToString()).Count(),
            };

            return verificationDocumentPageViewModel;
        }

        public VerificationDocumentViewModel GetUnverifiedUserById(string id)
        {
            var document = this.verificationDocumentRepository
                .AllAsNoTracking()
                .To<VerificationDocumentViewModel>()
                .FirstOrDefault(x => x.Id == id);

            if (document == null)
            {
                throw new ArgumentNullException("Document not found");
            }

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

        public WithdrawRequestPageViewModel GetAllWithdrawRequests(int pageNumber, int itemsPerPage)
        {
            var withdrawRequests = this.withdrawRepository
                .AllAsNoTracking()
                .Where(x => x.TransactionStatus == TransactionStatus.Pending.ToString())
                .OrderBy(x => x.CreatedOn)
                .Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                .To<WithdrawRequestViewModel>()
                .ToList();

            var pageViewModel = new WithdrawRequestPageViewModel
            {
                WithdrawRequestViewModels = withdrawRequests,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                DataCount = this.withdrawRepository
                .AllAsNoTracking()
                .Where(x => x.TransactionStatus == TransactionStatus.Pending.ToString()).Count(),
            };

            return pageViewModel;
        }

        public WithdrawRequestViewModel GetWithdrawRequestById(int? id)
        {
            var withdrawRequest = this.withdrawRepository
                .AllAsNoTracking()
                .To<WithdrawRequestViewModel>()
                .FirstOrDefault(x => x.Id == id);

            return withdrawRequest;
        }

        public async Task UpdateUserWithdrawRequestAsync(int id, WithdrawRequestInputModel input)
        {
            var withdrawRequest = this.withdrawRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            withdrawRequest.TransactionStatus = Enum.Parse(typeof(TransactionStatus), input.TransactionStatus).ToString();

            this.withdrawRepository.Update(withdrawRequest);
            await this.withdrawRepository.SaveChangesAsync();
        }
    }
}
