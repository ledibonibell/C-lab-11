namespace TicketsLibrary
{
    public class Ticker
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Price> Prices { get; set; } = new();
        public TodaysCondition Condition { get; set; } = new();
    }
}