using System.Collections.Generic;

namespace SmartBooks.Domains.SchoolEntities
{
    public class Block : SchoolBaseClass
    {
        public Block()
        {
            ClassRooms = new HashSet<ClassRoom>();
        }
        public ICollection<ClassRoom> ClassRooms { get; set; }
    }
}
