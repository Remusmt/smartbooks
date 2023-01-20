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
    public class InventoryItemService
    {
        private readonly Logger logger;
        private readonly IInventoryItemRepository<InventoryItem> repository;
        public InventoryItemService(
            Logger loger,
            IInventoryItemRepository<InventoryItem> itemRepository)
        {
            logger = loger;
            repository = itemRepository;
        }

        public async Task<InventoryItem> GetInventoryItemAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<List<InventoryItem>> GetInventoryItems(int companyId)
        {
            return await repository
                .ListAsync(new InventoryItemSpecification(companyId));
        }

        public async Task<InventoryItemsListModel> GetInventoryItems(
            int companyId, string sort, string order, int page, int pageSize)
        {
            int totalCount = await repository.CountAsync(new InventoryItemSpecification(companyId));
            List<InventoryItem> inventoryItems = await repository
                .ListAsync(new InventoryItemSpecification(companyId, sort, order, page, pageSize));
            return new InventoryItemsListModel
            {
                InventoryItems = inventoryItems,
                TotalCount = totalCount
            };
        }

        public async Task<InventoryItem> Add(InventoryItem inventoryItem)
        {
            if (inventoryItem.CompanyId == 0)
                throw new Exception("An error occured while saving payment terms");

            if (string.IsNullOrWhiteSpace(inventoryItem.Description))
                throw new Exception("Description cannot be blank");

            if (repository.DescriptionExists(inventoryItem.Description, inventoryItem.CompanyId))
                throw new Exception("A inventory item with a similar description already exists");

            if (string.IsNullOrWhiteSpace(inventoryItem.Code))
            {
                inventoryItem.Code = repository.GetCode(inventoryItem.CompanyId);
            }
            else
            {
                if (repository.CodeExists(inventoryItem.Code, inventoryItem.CompanyId))
                {
                    throw new Exception("A record with a similar code already exists");
                }
            }
            repository.Add(inventoryItem);
            await repository.SaveChangesAsync();

            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = inventoryItem.CompanyId,
                CreatedBy = inventoryItem.CreatedBy,
                CreatedByName = inventoryItem.CreatedByName,
                CreatedOn = inventoryItem.CreatedOn,
                RecordId = inventoryItem.Id,
                RecordType = RecordType.InventoryItem,
                SerializedRecord = logger.SeliarizeObject(inventoryItem)
            });
            return inventoryItem;
        }

        public async Task<InventoryItem> Update(
            InventoryItem inventoryItem,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(inventoryItem.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateDescription(inventoryItem.Id, inventoryItem.Description, inventoryItem.CompanyId))
            {
                throw new Exception($"Updating description with {inventoryItem.Description} would create a duplicate");
            }
            if (!string.IsNullOrWhiteSpace(inventoryItem.Code))
            {
                if (repository.DuplicateCode(inventoryItem.Id, inventoryItem.Code, inventoryItem.CompanyId))
                {
                    throw new Exception($"Updating code with {inventoryItem.Code} would create a duplicate");
                }
            }

            await repository.Update(inventoryItem);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = inventoryItem.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = inventoryItem.Id,
                RecordType = RecordType.InventoryItem,
                SerializedRecord = logger.SeliarizeObject(inventoryItem)
            });
            return inventoryItem;
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            InventoryItem inventoryItem = await repository.GetByIdAsync(Id);
            if (inventoryItem == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(inventoryItem);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = inventoryItem.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = inventoryItem.Id,
                RecordType = RecordType.InventoryItem,
                SerializedRecord = logger.SeliarizeObject(inventoryItem)
            });

            return Id;
        }
    }
}
