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
    public class TaxesService
    {
        private readonly Logger logger;
        private readonly ITaxRepository<Tax> repository;
        private readonly IRepository<TaxRate> taxRateRepository;
        public TaxesService(
            Logger loger,
            IRepository<TaxRate> taxRateRepo,
            ITaxRepository<Tax> taxRepository)
        {
            logger = loger;
            repository = taxRepository;
            taxRateRepository = taxRateRepo;
        }

        public async Task<Tax> GetTaxAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<TaxRate> GetTaxRateAsync(int id)
        {
            return await taxRateRepository.GetByIdAsync(id);
        }

        public async Task<Tax> GetTaxFromTaxRateAsync(int id)
        {
            return await repository.GetTaxFromTaxRateAsync(id);
        }

        public async Task<List<Tax>> GetTaxes(int companyId)
        {
            return await repository
                .ListAsync(new TaxSpecification(companyId));
        }

        public async Task<List<TaxRate>> GetTaxRates(int companyId)
        {
            return await taxRateRepository
                .ListAsync(new TaxRateSpecification(companyId));
        }

        public async Task<Tax> Add(Tax tax)
        {
            if (tax.CompanyId == 0)
                throw new Exception("An error occured while saving payment terms");

            if (string.IsNullOrWhiteSpace(tax.Code))
                throw new Exception("Code cannot be blank");

            if (string.IsNullOrWhiteSpace(tax.Description))
                throw new Exception("Description cannot be blank");

            if (repository.DescriptionExists(tax.Description, tax.CompanyId))
                throw new Exception("A tax with a similar description already exists");

            if (repository.CodeExists(tax.Code, tax.CompanyId))
                throw new Exception("A tax with a similar code already exists");

            repository.Add(tax);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = tax.CompanyId,
                CreatedBy = tax.CreatedBy,
                CreatedByName = tax.CreatedByName,
                CreatedOn = tax.CreatedOn,
                RecordId = tax.Id,
                RecordType = RecordType.Tax,
                SerializedRecord = logger.SeliarizeObject(tax)
            });
            return tax;
        }

        public async Task<TaxRate> CreateTaxRate(TaxRate taxRate)
        {
            if (string.IsNullOrWhiteSpace(taxRate.Description))
                throw new Exception("Description cannot be blank");

            if (await taxRateRepository
                .Any(e => e.Description == taxRate.Description && e.TaxId == taxRate.TaxId))
            {
                throw new Exception("A tax rate with a similar description already exist");
            }

            taxRateRepository.Add(taxRate);
            await taxRateRepository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = taxRate.CompanyId,
                CreatedBy = taxRate.CreatedBy,
                CreatedByName = taxRate.CreatedByName,
                CreatedOn = taxRate.CreatedOn,
                RecordId = taxRate.Id,
                RecordType = RecordType.TaxRate,
                SerializedRecord = logger.SeliarizeObject(taxRate)
            });
            return taxRate;
        }

        public async Task<Tax> Update(
            Tax tax,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(tax.Code))
                throw new Exception("Code cannot be blank");
            if (string.IsNullOrWhiteSpace(tax.Description))
                throw new Exception("Description cannot be blank");
            if (repository.DuplicateDescription(tax.Id, tax.Description, tax.CompanyId))
                throw new Exception($"Updating description with {tax.Description} would create a duplicate record");
            if (repository.DuplicateCode(tax.Id, tax.Code, tax.CompanyId))
                throw new Exception($"Updating code with {tax.Code} would create a duplicate record");

            await repository.Update(tax);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = tax.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = tax.Id,
                RecordType = RecordType.Tax,
                SerializedRecord = logger.SeliarizeObject(tax)
            });
            return tax;
        }

        public async Task<TaxRate> UpdateTaxRate(
           TaxRate taxRate,
           int userId,
           string userFullName,
           DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(taxRate.Description))
                throw new Exception("Description cannot be blank");

            if (await taxRateRepository
                .Any(e => e.Description == taxRate.Description 
                    && e.TaxId == taxRate.TaxId && e.Id != taxRate.Id))
            {
                throw new Exception($"Updating description with {taxRate.Description} would create a duplicate record");
            }
            await taxRateRepository.Update(taxRate);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = taxRate.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = taxRate.Id,
                RecordType = RecordType.TaxRate,
                SerializedRecord = logger.SeliarizeObject(taxRate)
            });
            return taxRate;
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Tax tax = await repository.GetByIdAsync(Id);
            if (tax == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(tax);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = tax.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = tax.Id,
                RecordType = RecordType.Tax,
                SerializedRecord = logger.SeliarizeObject(tax)
            });

            return Id;
        }

        public async Task<int> DeleteTaxRate(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            TaxRate taxRate = await taxRateRepository.GetByIdAsync(Id);
            if (taxRate == null)
            {
                throw new Exception("Record not found");
            }
            taxRateRepository.Delete(taxRate);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = taxRate.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = taxRate.Id,
                RecordType = RecordType.Tax,
                SerializedRecord = logger.SeliarizeObject(taxRate)
            });

            return Id;
        }

    }
}
