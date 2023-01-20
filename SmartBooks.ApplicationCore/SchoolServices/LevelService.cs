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
    public class LevelService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<Level> repository;
        public LevelService(Logger loger, ISchoolBaseRepository<Level> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<Level> GetLevelAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<Level>> GetLevelsAsync(int companyId)
        {
            return await repository
                .ListAsync(new SchoolLevelSpecification<Level>(companyId));
        }

        public async Task<List<Level>> GetLevelsWithClassRooms(int companyId)
        {
            return await repository
                .ListAsync(new SchoolLevelSpecification<Level>(companyId, true));
        }

        public async Task<Level> AddAsync(Level level)
        {
            if (level.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(level.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(level.Description, level.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(level);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = level.CompanyId,
                CreatedBy = level.CreatedBy,
                CreatedByName = level.CreatedByName,
                CreatedOn = level.CreatedOn,
                RecordId = level.Id,
                RecordType = RecordType.SchoolLevel,
                SerializedRecord = logger.SeliarizeObject(level)
            });
            return level;
        }

        public async Task<Level> UpdateAsync(
            Level level,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(level.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(level.Id, level.Description, level.CompanyId))
            {
                throw new Exception($"Updating description with {level.Description} would create a duplicate");
            }

            await repository.Update(level);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = level.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = level.Id,
                RecordType = RecordType.SchoolLevel,
                SerializedRecord = logger.SeliarizeObject(level)
            });
            return level;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Level level = await repository.GetByIdAsync(Id);
            if (level == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(level);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = level.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = level.Id,
                RecordType = RecordType.SchoolLevel,
                SerializedRecord = logger.SeliarizeObject(level)
            });

            return Id;
        }
    }
}
