using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Models;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Services
{
    public class InventoryService
    {
        public IRepository<BinCard> binCardRepository;
        public IRepository<UomConversion> unitConversionRepository;
        public IRepository<InventoryLedger> inventoryLedgerRepository;
        private readonly InventoryItemService inventoryItemService;
        public InventoryService(
            IRepository<BinCard> binCardRepo,
            IRepository<UomConversion> conversionRepository,
            IRepository<InventoryLedger> inventoryLedgerRepo,
            InventoryItemService itemService)
        {
            binCardRepository = binCardRepo;
            inventoryItemService = itemService;
            inventoryLedgerRepository = inventoryLedgerRepo;
            unitConversionRepository = conversionRepository;
        }
        public decimal GetUomConversion(int unitFrom, int unitTo)
        {
            if (unitFrom == unitTo)
            {
                return 1;
            }
            UomConversion uomConversion = unitConversionRepository
                .Find(e => (e.UnitofMeasureFromId == unitFrom && e.UnitofMeasureToId == unitTo) ||
                (e.UnitofMeasureFromId == unitTo && e.UnitofMeasureToId == unitFrom))
                .FirstOrDefault();
            // If no conversion is set the return 1 so no effect on units being converted
            if (uomConversion == null) return 1;
            // Then we are converting from small unit, so we need to divide
            if (uomConversion.UnitofMeasureFromId == unitFrom) return 1 / uomConversion.Factor;
            // If we get here we are converting from the bigger unit we need to multiply
            return uomConversion.Factor;
        }

        public async Task<bool> ReceiveInventory(
            InventoryDetails inventoryDetails,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            InventoryItem inventoryItem = await inventoryItemService.GetInventoryItemAsync(inventoryDetails.InventoryItemId);
            if (inventoryItem == null) throw new Exception("Invalid inventory item");
            // Get the units conversion factor
            decimal factor = GetUomConversion(inventoryDetails.UnitofMeasureId, inventoryItem.UnitofMeasureId);
            // Convert quantity
            decimal convertedQuantity = inventoryDetails.Quantity * factor;
            // Compute average cost
            // we use the details quantity to compute cost but the converted qty to compute average cost.
            // Because the cost entered was for the selected units, but the average is in the inventory item's units
            // Other costs is per received unit
            decimal averageCost = (inventoryItem.TotalAverageCost + ((inventoryDetails.Price + inventoryDetails.OtherCosts) * inventoryDetails.Quantity))
                / (convertedQuantity + inventoryItem.OnHand);
            // Get bincard if it exists
            BinCard binCard = GetBinCard(inventoryItem.Id, inventoryDetails.BinId);
            // If null this item as never been received b4 create
            if (binCard == null)
            {
                binCard = new BinCard
                {
                    BinId = inventoryDetails.BinId,
                    InventoryItemId = inventoryItem.Id,
                    Balance = convertedQuantity,
                    CreatedBy = inventoryDetails.UserId,
                    CreatedByName = inventoryDetails.UserName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    CompanyId = inventoryDetails.CompanyId
                };

                binCard.InventoryLedgers.Add(new InventoryLedger
                {
                    Balance = convertedQuantity,
                    Cost = inventoryDetails.Price * factor,
                    CostBF = inventoryItem.AverageCost,
                    CreatedBy = inventoryDetails.UserId,
                    CreatedByName = inventoryDetails.UserName,
                    CreatedOn = binCard.CreatedOn,
                    NewCost = averageCost,
                    Quantity = convertedQuantity,
                    SourceDocumentId = inventoryDetails.SourceDocumentId,
                    SourceDocumentReference = inventoryDetails.SourceDocumentReference,
                    SourceDocumentType = inventoryDetails.SourceDocumentType,
                    CompanyId = inventoryDetails.CompanyId,
                    TransactionDate = inventoryDetails.TransactionDate,
                    TransactionType = inventoryDetails.InventoryTransaction,
                    UnitofMeasureId = inventoryItem.UnitofMeasureId
                });

                binCardRepository.Add(binCard);
            }
            // Bincard exists update balance and create ledger entry
            else
            {
                InventoryLedger inventoryLedger = new InventoryLedger
                {
                    Balance = binCard.Balance + convertedQuantity,
                    BalanceBF = binCard.Balance,
                    BinCardId = binCard.Id,
                    Cost = inventoryDetails.Price * factor,
                    CostBF = inventoryItem.AverageCost,
                    CreatedBy = inventoryDetails.UserId,
                    CreatedByName = inventoryDetails.UserName,
                    CreatedOn = DateTimeOffset.Now,
                    NewCost = averageCost,
                    Quantity = convertedQuantity,
                    SourceDocumentId = inventoryDetails.SourceDocumentId,
                    SourceDocumentReference = inventoryDetails.SourceDocumentReference,
                    SourceDocumentType = inventoryDetails.SourceDocumentType,
                    CompanyId = inventoryDetails.CompanyId,
                    TransactionDate = inventoryDetails.TransactionDate,
                    TransactionType = inventoryDetails.InventoryTransaction,
                    UnitofMeasureId = inventoryItem.UnitofMeasureId
                };

                inventoryLedgerRepository.Add(inventoryLedger);

                binCard.Balance += convertedQuantity;
                await binCardRepository.Update(binCard);
            }

            // Update inventory item
            inventoryItem.OnHand += convertedQuantity;
            inventoryItem.AverageCost = averageCost;
            // Cost was for the entered units, so converting to item's unit's cost
            inventoryItem.LastCost = inventoryDetails.Price * factor;
            // Update item without saving
            await inventoryItemService.Update(inventoryItem, userId, userFullName, dateTimeOffset);
            // Return without saving this because, if we have more than one entry we can save as a batch
            // or entity transaction, so if transaction fails everything fails which is a good thing
            return true;
        }

        public async Task<bool> IssueInventory(
            InventoryDetails inventoryDetails,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            InventoryItem inventoryItem = await inventoryItemService.GetInventoryItemAsync(inventoryDetails.InventoryItemId);
            if (inventoryItem == null) throw new Exception("Invalid inventory item");
            // Get the units conversion factor
            decimal factor = GetUomConversion(inventoryDetails.UnitofMeasureId, inventoryItem.UnitofMeasureId);
            // Convert quantity
            decimal convertedQuantity = inventoryDetails.Quantity * factor;
            // Get bincard if it exists
            BinCard binCard = GetBinCard(inventoryItem.Id, inventoryDetails.BinId);
            // If no bincard to issue items from throw error
            if (binCard == null) throw new Exception("No items in selected location");
            // check if overdraw is allowed
            if (!inventoryDetails.AllowNegativeIssue)
            {
                // if overdraw is not allowed check if there is sufficient inventory
                // if not throw exception
                if (convertedQuantity > binCard.Balance) throw new Exception("Don't have sufficient items in selected location");
            }

            // Everything is okay or allowed proceed
            InventoryLedger inventoryLedger = new InventoryLedger
            {
                Balance = binCard.Balance - convertedQuantity,
                BalanceBF = binCard.Balance,
                BinCardId = binCard.Id,
                // OtherCosts is per the unit issued, so convert to item's unit's cost
                Cost = inventoryItem.AverageCost + (inventoryDetails.OtherCosts * factor),
                CostBF = inventoryItem.AverageCost,
                CreatedBy = inventoryDetails.UserId,
                CreatedByName = inventoryDetails.UserName,
                CreatedOn = DateTimeOffset.UtcNow,
                Quantity = convertedQuantity,
                SourceDocumentId = inventoryDetails.SourceDocumentId,
                SourceDocumentReference = inventoryDetails.SourceDocumentReference,
                SourceDocumentType = inventoryDetails.SourceDocumentType,
                CompanyId = inventoryDetails.CompanyId,
                TransactionDate = inventoryDetails.TransactionDate,
                TransactionType = inventoryDetails.InventoryTransaction,
                UnitofMeasureId = inventoryItem.UnitofMeasureId
            };

            inventoryLedgerRepository.Add(inventoryLedger);

            // Update bincard balance
            binCard.Balance -= convertedQuantity;
            await binCardRepository.Update(binCard);

            // Update inventory item onhand
            inventoryItem.OnHand -= convertedQuantity;
            // Update item without saving
            await inventoryItemService.Update(inventoryItem, userId, userFullName, dateTimeOffset);
            // Return without saving this because, if we have more than one entry we can save as a batch
            // or entity transaction, so if transaction fails everything fails which is a good thing
            return true;
        }

        private BinCard GetBinCard(
            int inventoryItem, 
            int binId)
        {
            BinCard binCard = binCardRepository
                .Find(e => e.InventoryItemId == inventoryItem
                && e.BinId == binId)
                .FirstOrDefault();
            return binCard;
        }

    }

}
