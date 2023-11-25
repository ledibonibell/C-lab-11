namespace TicketsLibrary
{
    public class TodaysCondition
    {
        public int id { get; set; }
        public int tickerId { get; set; }
        public string tickerName { get; set; }
        public Ticker ticker { get; set; }
        public decimal state { get; set; }
    }
}