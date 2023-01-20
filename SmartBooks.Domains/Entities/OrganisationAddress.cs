namespace SmartBooks.Domains.Entities
{
    public class OrganisationAddress : Address
    {
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
    }
}
