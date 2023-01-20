namespace SmartBooks.Domains.Entities
{
    public class DocumentSetting : AppBaseEntity
    {
        public string DocumentType { get; set; }
        public bool AutoGenerateReference { get; set; }
        public int NextReferenceNo { get; set; }
        public string ReferencePrefix { get; set; }
        public int ReferenceLength { get; set; }
        public bool RequireLevelOne { get; set; }
        public bool PrintLevelOne { get; set; }
        public string LevelOneTitle { get; set; }
        public bool RequireLevelTwo { get; set; }
        public bool PrintLevelTwo { get; set; }
        public string LevelTwoTitle { get; set; }
        public bool RequireLevelThree { get; set; }
        public bool PrintLevelThree { get; set; }
        public string LevelThreeTitle { get; set; }

    }
}
