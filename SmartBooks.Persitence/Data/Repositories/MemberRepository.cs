using Microsoft.EntityFrameworkCore;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.SaccoEntities;
using SmartBooks.Persitence.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooks.Persitence.Data.Repositories
{
    public class MemberRepository<T> : Repository<T>, IMemberRepository<T>
        where T : Member
    {
        public MemberRepository(SmartBooksContext context) : base(context)
        {
        }

        public bool DuplicateIdNumber(int id, string idNo, int companyId)
        {
            return smartBooksContext.Set<T>()
                  .Any(e => e.IndentificationNo == idNo
                  && e.CompanyId == companyId && e.Id != id);
        }

        public async Task<T> GetDetailedMember(int id)
        {
            return await smartBooksContext.Set<T>()
                .Include("MemberAttachments")
                .Include("MemberAttachments.Attachment")
                .Include("MemberApprovals")
                .Include("HomeAddress")
                .Include("PermanentAddress")
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<MemberAttachment> GetDetailedMemberAttachment(int id)
        {
            return await smartBooksContext.MemberAttachments
                .Include("Attachment")
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public bool IdNumberExists(string idNo, int companyId)
        {
            return smartBooksContext.Set<T>()
                .Any(e => e.IndentificationNo == idNo && e.CompanyId == companyId);

        }
    }
}
