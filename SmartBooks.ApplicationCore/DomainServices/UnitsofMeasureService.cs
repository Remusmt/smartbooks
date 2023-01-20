using SmartBooks.ApplicationCore.Models;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SmartBooks.ApplicationCore.DomainServices
{
    public class UnitsofMeasureService
    {
        private readonly Logger logger;
        private readonly IRepository<UomConversion> uomRepository;
        private readonly IUnitofMeasureRepository<UnitofMeasure> repository;

        public UnitsofMeasureService(
            Logger loger,
            IRepository<UomConversion> uomConversionRepository,
            IUnitofMeasureRepository<UnitofMeasure> unitofMeasureRepository
            )
        {
            logger = loger;
            repository = unitofMeasureRepository;
            uomRepository = uomConversionRepository;
        }

        public async Task<UnitofMeasure> GetUnitofMeasureAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<UomConversion> GetUomConversionAsync(int id)
        {
            return await uomRepository.GetByIdAsync(id);
        }

        public async Task<List<UnitofMeasureListModel>> GetUnitsofMeasure(int companyId)
        {
            List<UnitofMeasureListModel> unitofMeasureModels = new List<UnitofMeasureListModel>();
            List<UnitofMeasure> unitofMeasures = await repository
                .ListAsync(new UnitofMeasureSpecification(companyId));

            foreach (var uom in unitofMeasures)
            {
                unitofMeasureModels.Add(
                    new UnitofMeasureListModel
                    {
                        Abbreviation = uom.Abbreviation,
                        Description = uom.Description,
                        Id = uom.Id,
                        Type = uom.Type,
                        UomConversions = await GetUomConversionsByUomId(uom.Id)
                    });
            }

            return unitofMeasureModels;
        }

        public async Task<List<UomConversionModel>> GetUomConversionsByUomId(int uomId)
        {
            List<UomConversion> uomConversions = await uomRepository
                .ListAsync(new UomConversionSpecification(uomId));
            return uomConversions.Select(conversion =>
                new UomConversionModel
                {
                    Description = conversion.Description,
                    Factor = conversion.Factor,
                    Id = conversion.Id,
                    UnitofMeasureFromId = conversion.UnitofMeasureFromId,
                    UnitofMeasureToId = conversion.UnitofMeasureToId
                })
                .ToList();
        }

        public async Task<UnitofMeasureListModel> Add(UnitofMeasure unitofMeasure)
        {
            if (unitofMeasure.CompanyId == 0)
                throw new Exception("An error occured while saving unit of measure");

            if (string.IsNullOrWhiteSpace(unitofMeasure.Abbreviation))
                throw new Exception("Abbreviation cannot be blank");

            if (string.IsNullOrWhiteSpace(unitofMeasure.Description))
                throw new Exception("Description cannot be blank");

            if (repository.AbbreviationExists(unitofMeasure.Abbreviation, unitofMeasure.CompanyId))
                throw new Exception("A unit of measure with a similar abbreviation already exists");

            if (repository.DescriptionExists(unitofMeasure.Description, unitofMeasure.CompanyId))
                throw new Exception("A unit of measure with a similar description already exists");

            repository.Add(unitofMeasure);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = unitofMeasure.CompanyId,
                CreatedBy = unitofMeasure.CreatedBy,
                CreatedByName = unitofMeasure.CreatedByName,
                CreatedOn = unitofMeasure.CreatedOn,
                RecordId = unitofMeasure.Id,
                RecordType = RecordType.UnitofMeasure,
                SerializedRecord = logger.SeliarizeObject(unitofMeasure)
            });
            return GetUnitofMeasureListModel(unitofMeasure);
        }

        public async Task<UnitofMeasureListModel> Update(
            UnitofMeasure unitofMeasure,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(unitofMeasure.Abbreviation))
            {
                throw new Exception("Abbreviation cannot be blank");
            }
            if (string.IsNullOrWhiteSpace(unitofMeasure.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (repository.DuplicateAbbreviation(unitofMeasure.Id, unitofMeasure.Abbreviation, unitofMeasure.CompanyId))
            {
                throw new Exception($"Updating abbreviation with {unitofMeasure.Abbreviation} would create a duplicate");
            }
            if (repository.DuplicateDescription(unitofMeasure.Id, unitofMeasure.Description, unitofMeasure.CompanyId))
            {
                throw new Exception($"Updating description with {unitofMeasure.Description} would create a duplicate");
            }

            await repository.Update(unitofMeasure);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = unitofMeasure.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = unitofMeasure.Id,
                RecordType = RecordType.UnitofMeasure,
                SerializedRecord = logger.SeliarizeObject(unitofMeasure)
            });
            return GetUnitofMeasureListModel(unitofMeasure);
        }

        private UnitofMeasureListModel GetUnitofMeasureListModel(UnitofMeasure unitofMeasure)
        {
            return new UnitofMeasureListModel
            {
                Abbreviation = unitofMeasure.Abbreviation,
                Description = unitofMeasure.Description,
                Id = unitofMeasure.Id,
                Type = unitofMeasure.Type
            };
        }

        public async Task<int> Delete(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            UnitofMeasure unitofMeasure = await repository.GetByIdAsync(Id);
            if (unitofMeasure == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(unitofMeasure);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = unitofMeasure.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = unitofMeasure.Id,
                RecordType = RecordType.UnitofMeasure,
                SerializedRecord = logger.SeliarizeObject(unitofMeasure)
            });

            return Id;
        }

        public async Task<UomConversionModel> AddConversion(UomConversion model)
        {
            if (model.CompanyId == 0)
                throw new Exception("An error occured while saving unit conversion");

            if (string.IsNullOrWhiteSpace(model.Description))
                throw new Exception("Description cannot be blank");

            if (model.Factor == 0)
                throw new Exception("Invalid factor, factor cannot be zero");

            if (await uomRepository.Any(e => 
                (e.UnitofMeasureFromId == model.UnitofMeasureFromId && e.UnitofMeasureToId == model.UnitofMeasureToId) 
                || (e.UnitofMeasureFromId == model.UnitofMeasureToId && e.UnitofMeasureToId == model.UnitofMeasureFromId)))
            {
                throw new Exception("A similar conversion already exists");
            }

            uomRepository.Add(model);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = model.CompanyId,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.CreatedByName,
                CreatedOn = model.CreatedOn,
                RecordId = model.Id,
                RecordType = RecordType.UomConversion,
                SerializedRecord = logger.SeliarizeObject(model)
            });
            return new UomConversionModel
            {
                Description = model.Description,
                Factor = model.Factor,
                Id = model.Id,
                UnitofMeasureFromId = model.UnitofMeasureFromId,
                UnitofMeasureToId = model.UnitofMeasureToId
            };
        }

        public async Task<UomConversionModel> UpdateConversion(
            UomConversion model,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
                throw new Exception("Description cannot be blank");

            if (model.Factor == 0)
                throw new Exception("Invalid factor, factor cannot be zero");

            await uomRepository.Update(model);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = model.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = model.Id,
                RecordType = RecordType.UomConversion,
                SerializedRecord = logger.SeliarizeObject(model)
            });
            return new UomConversionModel
            {
                Description = model.Description,
                Factor = model.Factor,
                Id = model.Id,
                UnitofMeasureFromId = model.UnitofMeasureFromId,
                UnitofMeasureToId = model.UnitofMeasureToId
            };
        }

        public async Task<int> DeleteConversion(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            UomConversion uomConversion = await uomRepository.GetByIdAsync(Id);
            if (uomConversion == null)
            {
                throw new Exception("Record not found");
            }
            uomRepository.SoftDelete(uomConversion);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = uomConversion.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = uomConversion.Id,
                RecordType = RecordType.UomConversion,
                SerializedRecord = logger.SeliarizeObject(uomConversion)
            });

            return Id;
        }
    }
}
