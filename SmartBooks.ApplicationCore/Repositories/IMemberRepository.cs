using SmartBooks.Domains.SaccoEntities;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.Repositories
{
    public interface IMemberRepository<T> : IRepository<T>
        where T : Member
    {
        bool IdNumberExists(string idNo, int companyId);
        bool DuplicateIdNumber(int id, string idNo, int companyId);
        Task<T> GetDetailedMember(int id);
        Task<MemberAttachment> GetDetailedMemberAttachment(int id);
    }
}
