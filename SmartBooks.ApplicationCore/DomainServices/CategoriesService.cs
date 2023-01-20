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
    public class CategoriesService<T>
        where T : Category
    {
        private readonly Logger logger;
        private readonly ICategoryRepository<T> categoryRepository;

        public CategoriesService(
            Logger loger,
            ICategoryRepository<T> categoryRepo)
        {
            logger = loger;
            categoryRepository = categoryRepo;
        }

        public async Task<List<T>> GetCategories(int companyId)
        {
            return await categoryRepository
                .ListAsync(new CategoriesSpecification<T>(companyId));
        }

        public async Task<T> AddCategory(T category)
        {
            if (category.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(category.Description))
            {
                throw new Exception("Description cannot be blank");
            }

            if (string.IsNullOrWhiteSpace(category.Code))
            {
                category.Code = categoryRepository.GetCode(category.CompanyId);
            }
            else
            {
                if (categoryRepository.CodeExists(category.Code, category.CompanyId))
                {
                    throw new Exception("A record with a similar code already exists");
                }
            }

            if (categoryRepository.DescriptionExists(category.Description, category.CompanyId))
            {
                throw new Exception("A record with a similar description already exists");
            }

            categoryRepository.Add(category);
            await categoryRepository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = category.CompanyId,
                CreatedBy = category.CreatedBy,
                CreatedByName = category.CreatedByName,
                CreatedOn = category.CreatedOn,
                RecordId = category.Id,
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), typeof(T).Name),
                SerializedRecord = logger.SeliarizeObject(category)
            });
            return category;
        }

        public async Task<T> UpdateCategory(
            T category,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(category.Description))
            {
                throw new Exception("Description cannot be blank");
            }
            if (categoryRepository.DuplicateDescription(category.Id, category.Description, category.CompanyId))
            {
                throw new Exception($"Updating description with {category.Description} would create a duplicate");
            }
            if (!string.IsNullOrWhiteSpace(category.Code))
            {
                if (categoryRepository.DuplicateCode(category.Id, category.Code, category.CompanyId))
                {
                    throw new Exception($"Updating code with {category.Code} would create a duplicate");
                }
            }
            await categoryRepository.Update(category);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = category.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = category.Id,
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), typeof(T).Name),
                SerializedRecord = logger.SeliarizeObject(category)
            });
            return category;
        }

        public async Task<int> DeleteCategory(
            int Id,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            T category = await categoryRepository.GetByIdAsync(Id);
            if (category == null)
            {
                throw new Exception("Record not found");
            }
            categoryRepository.SoftDelete(category);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = category.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = category.Id,
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), typeof(T).Name),
                SerializedRecord = logger.SeliarizeObject(category)
            });

            return Id;
        }

        public async Task<T> GetCategoryAsync(int id)
        {
            return await categoryRepository.GetByIdAsync(id);
        }
    }
}
