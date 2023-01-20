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
    public class WarehouseService
    {
        private readonly Logger logger;
        private readonly IBinRepository<Bin> binRepository;
        private readonly IWarehouseRepository<Warehouse> warehouseRepository;

        public WarehouseService(
            Logger loger,
            IWarehouseRepository<Warehouse> warehouseRepo,
            IBinRepository<Bin> binRepo)
        {
            logger = loger;
            binRepository = binRepo;
            warehouseRepository = warehouseRepo;

        }

        public async Task<List<Warehouse>> GetWarehouses(int companyId)
        {
            return await warehouseRepository
                .ListAsync(new WarehouseSpecifications(companyId));
        }

        public async Task<List<Bin>> GetBins(int warehouseId)
        {
            return await binRepository
                .ListAsync(new BinsSpecification(warehouseId));
        }

        public async Task<Warehouse> AddWarehouse(Warehouse warehouse)
        {
            if (warehouse.CompanyId == 0) throw new Exception("An error occured while saving warehouse");

            if (string.IsNullOrWhiteSpace(warehouse.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (string.IsNullOrWhiteSpace(warehouse.Code))
            {
                warehouse.Code = warehouseRepository.GetCode(warehouse.CompanyId);
            }
            else
            {
                if (warehouseRepository.CodeExists(warehouse.Code, warehouse.CompanyId))
                {
                    throw new Exception("A warehouse with a similar code already exists");
                }
            }

            if (warehouseRepository.DescriptionExists(warehouse.Description, warehouse.CompanyId))
            {
                throw new Exception("A warehouse with a similar description already exists");
            }

            warehouseRepository.Add(warehouse);
            await warehouseRepository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = warehouse.CompanyId,
                CreatedBy = warehouse.CreatedBy,
                CreatedByName = warehouse.CreatedByName,
                CreatedOn = warehouse.CreatedOn,
                RecordId = warehouse.Id,
                RecordType = RecordType.Warehouse,
                SerializedRecord = logger.SeliarizeObject(warehouse)
            });
            return warehouse;
        }
        public async Task<Bin> AddBin(Bin bin)
        {
            if (bin.WarehouseId == 0) throw new Exception("Invalid warehouse");

            if (!await warehouseRepository.Any(e => e.Id == bin.WarehouseId && e.IsDeleted == false))
            {
                throw new Exception("Invalid warehouse");
            }

            if (string.IsNullOrWhiteSpace(bin.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (string.IsNullOrWhiteSpace(bin.Code))
            {
                bin.Code = binRepository.GetCode(bin.WarehouseId);
            }
            else
            {
                if (binRepository.CodeExists(bin.Code, bin.WarehouseId))
                {
                    throw new Exception("A bin with a similar code already exists");
                }
            }

            if (binRepository.DescriptionExists(bin.Description, bin.WarehouseId))
            {
                throw new Exception("A bin with a similar description already exists");
            }

            binRepository.Add(bin);
            await binRepository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = bin.CompanyId,
                CreatedBy = bin.CreatedBy,
                CreatedByName = bin.CreatedByName,
                CreatedOn = bin.CreatedOn,
                RecordId = bin.Id,
                RecordType = RecordType.Bin,
                SerializedRecord = logger.SeliarizeObject(bin)
            });
            return bin;
        }

        public async Task<Warehouse> UpdateWarehouse(
            Warehouse warehouse,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(warehouse.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (warehouseRepository.DuplicateDescription(warehouse.Id, warehouse.Description, warehouse.CompanyId))
            {
                throw new Exception($"Updating description with {warehouse.Description} would create a duplicate");
            }
            if (!string.IsNullOrWhiteSpace(warehouse.Code))
            {
                if (warehouseRepository.DuplicateCode(warehouse.Id, warehouse.Code, warehouse.CompanyId))
                {
                    throw new Exception($"Updating code with {warehouse.Code} would create a duplicate");
                }
            }
            await warehouseRepository.Update(warehouse);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = warehouse.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = warehouse.Id,
                RecordType = RecordType.Warehouse,
                SerializedRecord = logger.SeliarizeObject(warehouse)
            });
            return warehouse;
        }
        public async Task<Bin> UpdateBin(
            Bin bin,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(bin.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (binRepository.DuplicateDescription(bin.Id, bin.Description, bin.WarehouseId))
            {
                throw new Exception($"Updating description with {bin.Description} would create a duplicate");
            }
            if (!string.IsNullOrWhiteSpace(bin.Code))
            {
                if (binRepository.DuplicateCode(bin.Id, bin.Code, bin.WarehouseId))
                {
                    throw new Exception($"Updating code with {bin.Code} would create a duplicate");
                }
            }
            await binRepository.Update(bin);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = bin.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = bin.Id,
                RecordType = RecordType.Warehouse,
                SerializedRecord = logger.SeliarizeObject(bin)
            });
            return bin;
        }

        public async Task<int> DeleteWarehouse(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Warehouse warehouse = await warehouseRepository.GetByIdAsync(Id);
            if (warehouse == null)
            {
                throw new Exception("Record not found");
            }
            warehouseRepository.SoftDelete(warehouse);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = warehouse.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = warehouse.Id,
                RecordType = RecordType.Warehouse,
                SerializedRecord = logger.SeliarizeObject(warehouse)
            });

            return Id;
        }

        public async Task<int> DeleteBin(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            Bin bin = await binRepository.GetByIdAsync(Id);
            if (bin == null)
            {
                throw new Exception("Record not found");
            }
            binRepository.Delete(bin);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = bin.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = bin.Id,
                RecordType = RecordType.Bin,
                SerializedRecord = logger.SeliarizeObject(bin)
            });

            return Id;
        }

        public async Task<Warehouse> GetWarehouseAsync(int id)
        {
            return await warehouseRepository.GetByIdAsync(id);
        }

        public async Task<Bin> GetBinAsync(int id)
        {
            return await binRepository.GetByIdAsync(id);
        }


    }
}
