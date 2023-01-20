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
    public class OrganisationsServices<T>
        where T : Organisation
    {
        private readonly Logger logger;
        private readonly IOrganisationRepository<T> repository;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;
        public OrganisationsServices(
            Logger loger,
            IRepository<CompanyDefaults> companyDefaultsRepo,
            IOrganisationRepository<T> organisationRepository)
        {
            logger = loger;
            repository = organisationRepository;
            companyDefaultsRepository = companyDefaultsRepo;
        }

        public async Task<List<T>> GetOrganisations(int companyId)
        {
            return await repository
                .ListAsync(new OrganisationSpecification<T>(companyId));
        }

        public async Task<T> GetOrganisationAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<OrganisationAddress> GetOrganisationAddressAsync(int id)
        {
            return await repository.GetAddressById(id);
        }

        public async Task<OrganisationModel<T>> GetOrganisations(
          int companyId, string sort, string order, int page, int pageSize)
        {
            int totalCount = await repository.CountAsync(new OrganisationSpecification<T>(companyId));
            List<T> organisations = await repository
                .ListAsync(new OrganisationSpecification<T>(companyId, sort, order, page, pageSize));
            return new OrganisationModel<T>
            {
                Organisations = organisations,
                TotalCount = totalCount
            };
        }

        public async Task<T> Add(T organisation)
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
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), typeof(T).Name),
                SerializedRecord = logger.SeliarizeObject(organisation)
            });
            return organisation;
        }

        public async Task<T> Update(
            T organisation,
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
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), typeof(T).Name),
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
            T organisation = await repository.GetByIdAsync(Id);
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
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), typeof(T).Name),
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
                throw new Exception($"Invalid {typeof(T).Name}");
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
                throw new Exception($"Invalid {typeof(T).Name}");
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
