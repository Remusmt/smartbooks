using SmartBooks.Domains.Entities;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IOrganisationRepository<T> : IRepository<T>
        where T : Organisation
    {
        bool NameExists(string name, int companyId);
        bool DuplicateName(int id, string name, int companyId);
        bool LocationExists(string location, int organisationId);
        bool DuplicateLocation(int Id, string location, int organisationId);
        void CreateAddress(OrganisationAddress address);
        void UpdateAddress(OrganisationAddress address);
        void DeleteAddress(OrganisationAddress address);
        Task<OrganisationAddress> GetAddressById(int Id);

    }
}
