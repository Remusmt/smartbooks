using Microsoft.EntityFrameworkCore;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly SmartBooksContext smartBooksContext;
        public Repository(SmartBooksContext context)
        {
            smartBooksContext = context;
        }
        public void Add(T entity)
        {
            smartBooksContext.Set<T>().Add(entity);
        }
        public void AddRange(List<T> entities)
        {
            smartBooksContext.Set<T>().AddRange(entities);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await smartBooksContext.Set<T>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Delete(T entity)
        {
            if (entity.SystemGenerated)
            {
                throw new Exception("Deletion Failed. Record required by the system.");
            }
            smartBooksContext.Set<T>().Remove(entity);
        }

        public void SoftDelete(T entity)
        {
            if (entity.SystemGenerated)
                throw new Exception("Deletion Failed. Record required by the system.");

            entity.IsDeleted = true;
            smartBooksContext.Entry(smartBooksContext.Set<T>()
                .Find(entity.Id)).CurrentValues.SetValues(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            foreach (T entity in entities)
            {
                if (entity.SystemGenerated)
                    throw new Exception("Deletion Failed. Record required by the system.");
            }
            smartBooksContext.Set<T>().RemoveRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return smartBooksContext.Set<T>()
                .Where(predicate);
        }
        public virtual void Test()
        {

        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await smartBooksContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await smartBooksContext.Set<T>()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await smartBooksContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public int SaveChanges()
        {
            return smartBooksContext.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return smartBooksContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await smartBooksContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await smartBooksContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        //private void UpdateSoftDeleteStatuses()
        //{
        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.CurrentValues["isDeleted"] = false;
        //                break;
        //            case EntityState.Deleted:
        //                entry.State = EntityState.Modified;
        //                entry.CurrentValues["isDeleted"] = true;
        //                break;
        //        }
        //    }
        //}

        public async Task Update(T entity)
        {
            //Get entity in the database
            T oldEntity = await GetByIdAsync(entity.Id);
            //If non exist then there is non to update
            if (oldEntity == null) return;
            if (entity.UpdateCode < oldEntity.UpdateCode)
                throw new Exception("Record has been updated since you retrieved it");
                    
            entity.UpdateCode += 1;
            smartBooksContext.Entry(smartBooksContext.Set<T>()
                .Find(entity.Id)).CurrentValues.SetValues(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(smartBooksContext.Set<T>().AsQueryable(), spec);
        }
    }
}
