using SmartBooks.Domains.Enums;
using System.Collections.Generic;

namespace SmartBooks.Domains.SchoolEntities
{
    public class Level : SchoolBaseClass
    {
        public Level()
        {
            ClassRooms = new HashSet<ClassRoom>();
        }
        public SchoolLevel SchoolLevel { get; set; }
        public ICollection<ClassRoom> ClassRooms { get; set; }
    }
}
