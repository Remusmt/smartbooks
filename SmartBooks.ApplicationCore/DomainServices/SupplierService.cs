using SmartBooks.ApplicationCore.Models;
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
    public class SupplierService
    {
        private readonly Logger logger;
        private readonly IOrganisationRepository<Supplier> repository;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;
        public SupplierService(
            Logger loger,
            IRepository<CompanyDefaults> companyDefaultsRepo,
            IOrganisationRepository<Supplier> organisationRepository)
        {
            logger = loger;
            repository = organisationRepository;
            companyDefaultsRepository = companyDefaultsRepo;
        }

        public async Task<List<Supplier>> GetOrganisations(int companyId)
        {
            return await repository
                .ListAsync(new OrganisationSpecification<Supplier>(companyId));
        }

        public async Task<List<Supplier>> GetTaxAngencies(int companyId)
        {
            return await repository
                .ListAsync(new SupplierSpecification(companyId, true));
        }

        public async Task<Supplier> GetOrganisationAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<OrganisationAddress> GetOrganisationAddressAsync(int id)
        {
            return await repository.GetAddressById(id);
        }

        public async Task<SupplierListModel> GetOrganisations(
          int companyId, string sort, string order, int page, int pageSize)
        {
            int totalCount = await repository.CountAsync(new OrganisationSpecification<Supplier>(companyId));
            List<Supplier> organisations = await repository
                .ListAsync(new OrganisationSpecification<Supplier>(companyId, sort, order, page, pageSize));
            return new SupplierListModel
            {
                Organisations = organisations,
                TotalCount = totalCount
            };
        }

        public async Task<Supplier> Add(Supplier organisation)
        {
            if (organisation.CompanyId == 0) throw new Exception("An error occured while saving");
            if (organisation.CurrencyId == 0)
            {
                CompanyDefaults companyDefaults = await companyDefaultsRepository
                    .FirstOrDefaultAsync(e => e.CompanyId == organisation.CompanyId);
                organisation.CurrencyId = companyDefaults.DefaultCurrency;
            }

            if (string.IsNullOrWhiteSpace(organisation.Name))
            {
                throw new Exception("Name cannot be blank");
            }


            if (repository.NameExists(organisation.Name, organisation.CompanyId))
            {
                throw new Exception("A record with a similar name already exists");
            }

            repository.Add(organisation);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = organisation.CompanyId,
                CreatedBy = organisation.CreatedBy,
                CreatedByName = organisation.CreatedByName,
                CreatedOn = organisation.CreatedOn,
                RecordId = organisation.Id,
                RecordType = RecordType.Supplier,
                SerializedRecord = logger.SeliarizeObject(organisation)
            });
            return organisation;
        }

        public async Task<Supplier> Update(
            Supplier organisation,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(organisation.Name))
            {
                throw new Exception("Name cannot be blank");
            }
            if (repository.DuplicateName(organisation.Id, organisation.Name, organisation.CompanyId))
            {
                throw new Exception($"Updating name with {organisation.Name} would create a duplicate");
            }

            await repository.Update(organisation);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = organisation.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = organisation.Id,
                RecordType = RecordType.Supplier,
                SerializedRecord = logger.SeliarizeObject(organisation)
            });
            return organisation;
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Supplier organisation = await repository.GetByIdAsync(Id);
            if (organisation == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(organisation);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = organisation.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = organisation.Id,
                RecordType = RecordType.Supplier,
                SerializedRecord = logger.SeliarizeObject(organisation)
            });

            return Id;
        }

        public async Task<OrganisationAddress> CreateAddress(
            OrganisationAddress address,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(address.Location))
            {
                throw new Exception("Location cannot be blank");
            }
            Organisation organisation = await GetOrganisationAsync(address.OrganisationId);
            if (organisation == null)
                throw new Exception($"Invalid Supplier");
            if (repository.LocationExists(address.Location, organisation.Id))
                throw new Exception("An address with a similar location already exist");
            repository.CreateAddress(address);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = organisation.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = address.Id,
                RecordType = RecordType.OrganisationAddress,
                SerializedRecord = logger.SeliarizeObject(address)
            });
            return address;
        }

        public async Task<OrganisationAddress> UpdateAddress(
         OrganisationAddress address,
         int userId,
         string userFullName,
         DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(address.Location))
            {
                throw new Exception("Location cannot be blank");
            }

            Organisation organisation = await GetOrganisationAsync(address.OrganisationId);
            if (organisation == null)
                throw new Exception($"Invalid supplier");
            if (repository.DuplicateLocation(address.Id, address.Location, organisation.Id))
                throw new Exception("There is another address with a similar location");
            repository.UpdateAddress(address);

            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = organisation.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = address.Id,
                RecordType = RecordType.OrganisationAddress,
                SerializedRecord = logger.SeliarizeObject(address)
            });
            return address;
        }

        public async Task<int> DeleteAddress(
        int Id,
        int userId,
        int companyId,
        string userFullName,
        DateTimeOffset dateTimeOffset)
        {
            OrganisationAddress address = await repository.GetAddressById(Id);
            if (address == null)
            {
                throw new Exception("Record not found");
            }
            repository.DeleteAddress(address);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = companyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = address.Id,
                RecordType = RecordType.OrganisationAddress,
                SerializedRecord = logger.SeliarizeObject(address)
            });

            return Id;
        }
    }
}
