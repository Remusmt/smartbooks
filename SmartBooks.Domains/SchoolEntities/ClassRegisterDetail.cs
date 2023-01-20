using SmartBooks.Domains.Entities;

namespace SmartBooks.Domains.SchoolEntities
{
    public class ClassRegisterDetail : AppBaseEntity
    {
        public int ClassRegisterId { get; set; }
        public int StudentId { get; set; }
        public ClassRegister ClassRegister { get; set; }
        public Student Student { get; set; }
    }
}
