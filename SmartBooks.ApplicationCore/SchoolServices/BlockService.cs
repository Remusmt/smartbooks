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
    public class BlockService
    {
        private readonly Logger logger;
        private readonly ISchoolBaseRepository<Block> repository;
        public BlockService(Logger loger, ISchoolBaseRepository<Block> repo)
        {
            logger = loger;
            repository = repo;
        }

        public async Task<Block> GetBlockAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<Block>> GetBlocksAsync(int companyId)
        {
            return await repository
                .ListAsync(new SchoolBlockSpecification<Block>(companyId));
        }

        public async Task<List<Block>> GetBlocksWithClassRooms(int companyId)
        {
            return await repository
                .ListAsync(new SchoolBlockSpecification<Block>(companyId, true));
        }

        public async Task<Block> AddAsync(Block block)
        {
            if (block.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(block.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (repository.DescriptionExists(block.Description, block.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            repository.Add(block);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = block.CompanyId,
                CreatedBy = block.CreatedBy,
                CreatedByName = block.CreatedByName,
                CreatedOn = block.CreatedOn,
                RecordId = block.Id,
                RecordType = RecordType.SchoolBlock,
                SerializedRecord = logger.SeliarizeObject(block)
            });
            return block;
        }

        public async Task<Block> UpdateAsync(
            Block block,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(block.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(block.Id, block.Description, block.CompanyId))
            {
                throw new Exception($"Updating description with {block.Description} would create a duplicate");
            }

            await repository.Update(block);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = block.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = block.Id,
                RecordType = RecordType.SchoolBlock,
                SerializedRecord = logger.SeliarizeObject(block)
            });
            return block;
        }

        public async Task<int> DeleteAsync(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Block block = await repository.GetByIdAsync(Id);
            if (block == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(block);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = block.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = block.Id,
                RecordType = RecordType.SchoolBlock,
                SerializedRecord = logger.SeliarizeObject(block)
            });

            return Id;
        }

    }
}
