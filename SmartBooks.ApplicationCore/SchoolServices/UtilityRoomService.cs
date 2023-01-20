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
    public class UtilityRoomService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<UtilityRoom> repository;
        public UtilityRoomService(Logger loger, ISchoolBaseRepository<UtilityRoom> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<UtilityRoom> GetUtilityRoomAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<UtilityRoom>> GetUtilityRoomsAsync(int companyId)
        {
            return await repository
                .ListAsync(new SchoolBaseSpecification<UtilityRoom>(companyId));
        }

        public async Task<UtilityRoom> AddAsync(UtilityRoom utilityRoom)
        {
            if (utilityRoom.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(utilityRoom.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(utilityRoom.Description, utilityRoom.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(utilityRoom);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = utilityRoom.CompanyId,
                CreatedBy = utilityRoom.CreatedBy,
                CreatedByName = utilityRoom.CreatedByName,
                CreatedOn = utilityRoom.CreatedOn,
                RecordId = utilityRoom.Id,
                RecordType = RecordType.UtilityRoom,
                SerializedRecord = logger.SeliarizeObject(utilityRoom)
            });
            return utilityRoom;
        }

        public async Task<UtilityRoom> UpdateAsync(
            UtilityRoom utilityRoom,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(utilityRoom.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(utilityRoom.Id, utilityRoom.Description, utilityRoom.CompanyId))
            {
                throw new Exception($"Updating description with {utilityRoom.Description} would create a duplicate record");
            }

            await repository.Update(utilityRoom);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = utilityRoom.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = utilityRoom.Id,
                RecordType = RecordType.UtilityRoom,
                SerializedRecord = logger.SeliarizeObject(utilityRoom)
            });
            return utilityRoom;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            UtilityRoom utilityRoom = await repository.GetByIdAsync(Id);
            if (utilityRoom == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(utilityRoom);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = utilityRoom.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = utilityRoom.Id,
                RecordType = RecordType.UtilityRoom,
                SerializedRecord = logger.SeliarizeObject(utilityRoom)
            });

            return Id;
        }
    }
}
