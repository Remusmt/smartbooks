using SmartBooks.Domains.Entities;
using System.Collections.Generic;

namespace SmartBooks.Domains.SchoolEntities
{
    public class ClassRegister : AppBaseEntity
    {
        public ClassRegister()
        {
            ClassRegisterDetails = new HashSet<ClassRegisterDetail>();
        }
        public int ClassRoomId { get; set; }
        public int SchoolTermId { get; set; }
        public int ClassTeacherId { get; set; }

        public ClassRoom ClassRoom { get; set; }
        public SchoolTerm SchoolTerm { get; set; }
        public Teacher ClassTeacher { get; set; }
        public ICollection<ClassRegisterDetail> ClassRegisterDetails { get; set; }
    }
}
