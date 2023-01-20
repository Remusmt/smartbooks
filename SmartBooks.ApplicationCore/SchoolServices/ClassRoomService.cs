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
    public class ClassRoomService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<ClassRoom> repository;
        public ClassRoomService(Logger loger, ISchoolBaseRepository<ClassRoom> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<ClassRoom> GetClassRoomAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<ClassRoom>> GetClassRoomsAsync(int companyId)
        {
            return await repository
                .ListAsync(new ClassRoomSpecification<ClassRoom>(companyId));
        }

        public async Task<List<ClassRoom>> GetDetailedClassRoomsAsync(int companyId)
        {
            return await repository
                .ListAsync(new ClassRoomSpecification<ClassRoom>(companyId, true));
        }

        public async Task<ClassRoom> AddAsync(ClassRoom classRoom)
        {
            if (classRoom.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(classRoom.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(classRoom.Description, classRoom.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(classRoom);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = classRoom.CompanyId,
                CreatedBy = classRoom.CreatedBy,
                CreatedByName = classRoom.CreatedByName,
                CreatedOn = classRoom.CreatedOn,
                RecordId = classRoom.Id,
                RecordType = RecordType.ClassRoom,
                SerializedRecord = logger.SeliarizeObject(classRoom)
            });
            return classRoom;
        }

        public async Task<ClassRoom> UpdateAsync(
            ClassRoom classRoom,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(classRoom.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(classRoom.Id, classRoom.Description, classRoom.CompanyId))
            {
                throw new Exception($"Updating description with {classRoom.Description} would create a duplicate");
            }

            await repository.Update(classRoom);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = classRoom.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = classRoom.Id,
                RecordType = RecordType.ClassRoom,
                SerializedRecord = logger.SeliarizeObject(classRoom)
            });
            return classRoom;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            ClassRoom classRoom = await repository.GetByIdAsync(Id);
            if (classRoom == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(classRoom);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = classRoom.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = classRoom.Id,
                RecordType = RecordType.ClassRoom,
                SerializedRecord = logger.SeliarizeObject(classRoom)
            });

            return Id;
        }
    }
}
