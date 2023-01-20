namespace SmartBooks.Domains.SchoolEntities
{
    public class ClassRoom : SchoolBaseClass
    {
        public int Capacity { get; set; }
        public int BlockId { get; set; }
        public int LevelId { get; set; }

        public Block Block { get; set; }
        public Level Level { get; set; }
    }
}
