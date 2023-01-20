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
    public class CustomerService
    {
        private readonly Logger logger;
        private readonly IOrganisationRepository<Customer> repository;
        public CustomerService(
            Logger loger,
            IOrganisationRepository<Customer> organisationRepository)
        {
            logger = loger;
            repository = organisationRepository;
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await repository.GetByIdAsync(id);

        }
        public async Task<List<Customer>> GetCustomers(int companyId)
        {
            return await repository
                .ListAsync(new OrganisationSpecification<Customer>(companyId));
        }

        public async Task<CustomerListModel> GetCustomers(
            int companyId, string sort, string order, int page, int pageSize)
        {
            int totalCount = await repository.CountAsync(new OrganisationSpecification<Customer>(companyId));
            List<Customer> customers = await repository
                .ListAsync(new OrganisationSpecification<Customer>(companyId, sort, order, page, pageSize));
            return new CustomerListModel
            {
                Customers = customers,
                TotalCount = totalCount
            };
        }

        public async Task<Customer> Add(Customer customer)
        {
            if (customer.CompanyId == 0) throw new Exception("An error occured while saving");

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                throw new Exception("Name cannot be blank");
            }


            if (repository.NameExists(customer.Name, customer.CompanyId))
            {
                throw new Exception("A record with a similar name already exists");
            }

            repository.Add(customer);
            await repository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = customer.CompanyId,
                CreatedBy = customer.CreatedBy,
                CreatedByName = customer.CreatedByName,
                CreatedOn = customer.CreatedOn,
                RecordId = customer.Id,
                RecordType = RecordType.Customer,
                SerializedRecord = logger.SeliarizeObject(customer)
            });
            return customer;
        }

        public async Task<Customer> Update(
            Customer customer,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                throw new Exception("Name cannot be blank");
            }
            if (repository.DuplicateName(customer.Id, customer.Name, customer.CompanyId))
            {
                throw new Exception($"Updating name with {customer.Name} would create a duplicate");
            }

            await repository.Update(customer);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Update,
                CompanyId = customer.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = customer.Id,
                RecordType = RecordType.Customer,
                SerializedRecord = logger.SeliarizeObject(customer)
            });
            return customer;
        }

        public async Task<int> Delete(
           int Id,
           int userId,
           string userFullName,
           DateTimeOffset dateTimeOffset)
        {
            Customer customer = await repository.GetByIdAsync(Id);
            if (customer == null)
            {
                throw new Exception("Record not found");
            }
            repository.SoftDelete(customer);
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Delete,
                CompanyId = customer.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = customer.Id,
                RecordType = RecordType.Customer,
                SerializedRecord = logger.SeliarizeObject(customer)
            });

            return Id;
        }

    }
}
