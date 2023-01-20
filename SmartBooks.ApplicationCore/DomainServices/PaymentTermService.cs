using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.DomainServices
{
    public class PaymentTermService
    {
        private readonly Logger logger;
        private readonly IPaymentTermRepository<PaymentTerm> repository;

        public PaymentTermService(
            Logger loger,
            IPaymentTermRepository<PaymentTerm> paymentTermRepository
            )
        {
            logger = loger;
            repository = paymentTermRepository;
        }

        public async Task<PaymentTerm> GetPaymentTermAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<PaymentTerm>> GetPaymentTerms(int companyId)
        {
            return await repository
                .ListAsync(new PaymentTermSpecification(companyId));
        }

        public async Task<PaymentTerm> Add(PaymentTerm paymentTerm)
        {
            if (paymentTerm.CompanyId == 0) 
                throw new Exception("An error occured while saving payment terms");

            if (string.IsNullOrWhiteSpace(paymentTerm.Description))
                throw new Exception("Description cannot be blank");

            if (repository.DescriptionExists(paymentTerm.Description, paymentTerm.CompanyId))
                throw new Exception("A payment terms with a similar description already exists");

            repository.Add(paymentTerm);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = paymentTerm.CompanyId,
                CreatedBy = paymentTerm.CreatedBy,
                CreatedByName = paymentTerm.CreatedByName,
                CreatedOn = paymentTerm.CreatedOn,
                RecordId = paymentTerm.Id,
                RecordType = RecordType.PaymentTerm,
                SerializedRecord = logger.SeliarizeObject(paymentTerm)
            });
            return paymentTerm;
        }

        public async Task<PaymentTerm> Update(
            PaymentTerm paymentTerm,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(paymentTerm.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(paymentTerm.Id, paymentTerm.Description, paymentTerm.CompanyId))
            {
                throw new Exception($"Updating description with {paymentTerm.Description} would create a duplicate");
            }

            await repository.Update(paymentTerm);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = paymentTerm.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = paymentTerm.Id,
                RecordType = RecordType.PaymentTerm,
                SerializedRecord = logger.SeliarizeObject(paymentTerm)
            });
            return paymentTerm;
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            PaymentTerm paymentTerm = await repository.GetByIdAsync(Id);
            if (paymentTerm == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(paymentTerm);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = paymentTerm.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = paymentTerm.Id,
                RecordType = RecordType.PaymentTerm,
                SerializedRecord = logger.SeliarizeObject(paymentTerm)
            });

            return Id;
        }

    }
}
