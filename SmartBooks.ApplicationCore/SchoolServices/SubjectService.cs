using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using SmartBooks.Domains.SchoolEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.SchoolServices
{
    public class SubjectService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<Subject> repository;
        public SubjectService(Logger loger, ISchoolBaseRepository<Subject> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<Subject> GetSubjectAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<Subject>> GetSubjectsAsync(int companyId)
        {
            return await repository
                .ListAsync(new SchoolBaseSpecification<Subject>(companyId));
        }

        public async Task<Subject> AddAsync(Subject subject)
        {
            if (subject.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(subject.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(subject.Description, subject.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(subject);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = subject.CompanyId,
                CreatedBy = subject.CreatedBy,
                CreatedByName = subject.CreatedByName,
                CreatedOn = subject.CreatedOn,
                RecordId = subject.Id,
                RecordType = RecordType.Subject,
                SerializedRecord = logger.SeliarizeObject(subject)
            });
            return subject;
        }

        public async Task<Subject> UpdateAsync(
            Subject subject,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(subject.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(subject.Id, subject.Description, subject.CompanyId))
            {
                throw new Exception($"Updating description with {subject.Description} would create a duplicate record");
            }

            await repository.Update(subject);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = subject.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = subject.Id,
                RecordType = RecordType.Subject,
                SerializedRecord = logger.SeliarizeObject(subject)
            });
            return subject;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Subject subject = await repository.GetByIdAsync(Id);
            if (subject == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(subject);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = subject.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = subject.Id,
                RecordType = RecordType.Subject,
                SerializedRecord = logger.SeliarizeObject(subject)
            });

            return Id;
        }
    }
}
