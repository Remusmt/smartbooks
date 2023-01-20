using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using SmartBooks.Domains.SchoolEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.SchoolServices
{
    public class DormitoryService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<Dormitory> repository;
        public DormitoryService(Logger loger, ISchoolBaseRepository<Dormitory> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<Dormitory> GetDormitoryAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<Dormitory>> GetDormitoriesAsync(int companyId)
        {
            return await repository
                .ListAsync(new SchoolBaseSpecification<Dormitory>(companyId));
        }

        public async Task<Dormitory> AddAsync(Dormitory dormitory)
        {
            if (dormitory.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(dormitory.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(dormitory.Description, dormitory.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(dormitory);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = dormitory.CompanyId,
                CreatedBy = dormitory.CreatedBy,
                CreatedByName = dormitory.CreatedByName,
                CreatedOn = dormitory.CreatedOn,
                RecordId = dormitory.Id,
                RecordType = RecordType.Dormitory,
                SerializedRecord = logger.SeliarizeObject(dormitory)
            });
            return dormitory;
        }

        public async Task<Dormitory> UpdateAsync(
            Dormitory dormitory,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(dormitory.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(dormitory.Id, dormitory.Description, dormitory.CompanyId))
            {
                throw new Exception($"Updating description with {dormitory.Description} would create a duplicate record");
            }

            await repository.Update(dormitory);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = dormitory.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = dormitory.Id,
                RecordType = RecordType.Dormitory,
                SerializedRecord = logger.SeliarizeObject(dormitory)
            });
            return dormitory;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Dormitory dormitory = await repository.GetByIdAsync(Id);
            if (dormitory == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(dormitory);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = dormitory.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = dormitory.Id,
                RecordType = RecordType.Dormitory,
                SerializedRecord = logger.SeliarizeObject(dormitory)
            });

            return Id;
        }
    }
}
