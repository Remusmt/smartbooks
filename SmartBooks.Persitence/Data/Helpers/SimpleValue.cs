namespace SmartBooks.Persitence.Data.Helpers
{
    public class SimpleValue
    {
        public string Value { get; set; }
        public override string ToString()
        {
            return Value;
        }
    }
}
