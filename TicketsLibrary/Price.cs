namespace TicketsLibrary
{
    public class Price
    {
        public int id { get; set; }
        public int tickerId { get; set; }
        public string tickerName { get; set; }
        public Ticker? ticker { get; set; }
        public decimal price { get; set; }
        public DateOnly date { get; set; }
    }
}