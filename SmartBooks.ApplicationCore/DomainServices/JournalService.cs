using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.DomainServices
{
    public class JournalService
    {
        private readonly Logger logger;
        private readonly IJournalRepository<Journal> repository;
        public JournalService(
            Logger loger,
            IJournalRepository<Journal> repo)
        {
            logger = loger;
            repository = repo;
        }

        public bool ReferenceNumberExists(string referenceNumber, int companyId)
        {
            return repository.ReferenceNumberExists(referenceNumber, companyId);
        }

        public async Task<Journal> Add(
            Journal journal,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset,
            bool doNotLog = false)
        {
            repository.Add(journal);
            if (doNotLog)
            {
                await logger.Log(new AuditLog
                {
                    ActionType = ActionType.Update,
                    CompanyId = journal.CompanyId,
                    CreatedBy = userId,
                    CreatedByName = userFullName,
                    CreatedOn = dateTimeOffset,
                    RecordId = journal.Id,
                    RecordType = RecordType.InventoryItem,
                    SerializedRecord = logger.SeliarizeObject(journal)
                });
            }
            return journal;
        }

        public async Task<Journal> Update(
            Journal journal,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset,
            bool doNotLog = false)
        {
            await repository.Update(journal);
            if (doNotLog)
            {
                await logger.Log(new AuditLog
                {
                    ActionType = ActionType.Update,
                    CompanyId = journal.CompanyId,
                    CreatedBy = userId,
                    CreatedByName = userFullName,
                    CreatedOn = dateTimeOffset,
                    RecordId = journal.Id,
                    RecordType = RecordType.InventoryItem,
                    SerializedRecord = logger.SeliarizeObject(journal)
                });
            }           
            return journal;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Journal> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Journal> GetDetailedByIdAsync(int id)
        {
            return await repository.GetDetailedByIdAsync(id);
        }
    }
}
