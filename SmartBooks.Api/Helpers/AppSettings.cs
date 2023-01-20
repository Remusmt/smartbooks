namespace SmartBooks.Api.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public DbmsType DbmsType { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}
