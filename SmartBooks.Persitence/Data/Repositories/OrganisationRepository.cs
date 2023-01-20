using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class OrganisationRepository<T> : Repository<T>, IOrganisationRepository<T>
        where T : Organisation
    {
        public OrganisationRepository(SmartBooksContext context) : base(context)
        {
        }

        public void CreateAddress(OrganisationAddress address)
        {
            smartBooksContext.OrganisationAddresses.Add(address);
        }

        public void UpdateAddress(OrganisationAddress address)
        {
            smartBooksContext.Entry(smartBooksContext.OrganisationAddresses
                .Find(address.Id)).CurrentValues.SetValues(address);
        }

        public void DeleteAddress(OrganisationAddress address)
        {
            smartBooksContext.OrganisationAddresses.Remove(address);
        }

        public bool DuplicateLocation(int Id, string location, int organisationId)
        {
            return smartBooksContext.OrganisationAddresses
               .Any(e => e.Location == location && e.OrganisationId == organisationId
               && e.Id != Id && !e.IsDeleted);
        }

        public bool DuplicateName(int id, string name, int companyId)
        {
            return smartBooksContext.Set<T>()
               .Any(e => e.Name == name && e.CompanyId == companyId
               && e.Id != id && !e.IsDeleted);
        }

        public async Task<OrganisationAddress> GetAddressById(int Id)
        {
            return await smartBooksContext.OrganisationAddresses.FindAsync(Id);
        }

        public bool LocationExists(string location, int organisationId)
        {
            return smartBooksContext.OrganisationAddresses
                .Any(e => e.Location == location && e.OrganisationId == organisationId 
                && !e.IsDeleted);
        }

        public bool NameExists(string name, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.Name == name && e.CompanyId == companyId && !e.IsDeleted);
        }

    }
}
