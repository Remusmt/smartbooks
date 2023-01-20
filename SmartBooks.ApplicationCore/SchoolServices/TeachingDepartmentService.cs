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
    public class TeachingDepartmentService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<TeachingDepartment> repository;
        public TeachingDepartmentService(Logger loger, ISchoolBaseRepository<TeachingDepartment> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<TeachingDepartment> GetTeachingDepartmentAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<TeachingDepartment>> GetTeachingDepartmentsAsync(int companyId)
        {
            return await repository
                .ListAsync(new SchoolBaseSpecification<TeachingDepartment>(companyId));
        }

        public async Task<TeachingDepartment> AddAsync(TeachingDepartment department)
        {
            if (department.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(department.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(department.Description, department.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(department);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = department.CompanyId,
                CreatedBy = department.CreatedBy,
                CreatedByName = department.CreatedByName,
                CreatedOn = department.CreatedOn,
                RecordId = department.Id,
                RecordType = RecordType.TeachingDepartment,
                SerializedRecord = logger.SeliarizeObject(department)
            });
            return department;
        }

        public async Task<TeachingDepartment> UpdateAsync(
            TeachingDepartment department,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(department.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(department.Id, department.Description, department.CompanyId))
            {
                throw new Exception($"Updating description with {department.Description} would create a duplicate record");
            }

            await repository.Update(department);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = department.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = department.Id,
                RecordType = RecordType.TeachingDepartment,
                SerializedRecord = logger.SeliarizeObject(department)
            });
            return department;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            TeachingDepartment department = await repository.GetByIdAsync(Id);
            if (department == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(department);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = department.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = department.Id,
                RecordType = RecordType.TeachingDepartment,
                SerializedRecord = logger.SeliarizeObject(department)
            });

            return Id;
        }
    }
}
