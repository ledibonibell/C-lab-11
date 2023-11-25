using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    public class Tickers
    {
        public int Id { get; set; }
        public string TickerSymbol { get; set; }
    }

    public class Prices
    {
        public int Id { get; set; }
        public int TickerId { get; set; }
        public double Price { get; set; }
        public DateTimeOffset Date { get; set; }
    }

    public class TodaysCondition
    {
        public int Id { get; set; }
        public int TickerId { get; set; }
        public bool State { get; set; }
    }

    public class StockContext : DbContext
    {
        public DbSet<Tickers> Tickers { get; set; }
        public DbSet<Prices> Prices { get; set; }
        public DbSet<TodaysCondition> TodaysCondition { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=StockApp;User Id=sa;Password=MyStr0ngPassword!;");
        }
    }

    public class Program
    {
        async public static void Main()
        {
            var tcpListener = new TcpListener(IPAddress.Any, 8888);

            tcpListener.Start();
            try
            {
                while (true)
                {
                    using var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    var stream = tcpClient.GetStream();
                    byte[] data = new byte[256];
                    await stream.ReadAsync(data);
                    var tickerSymbol = Encoding.UTF8.GetString(data);

                    using (var context = new StockContext())
                    {
                        // Create
                        var newTicker = new Tickers { TickerSymbol = tickerSymbol };
                        context.Tickers.Add(newTicker);
                        context.SaveChanges();
                        Console.WriteLine($"Ticker created: {newTicker.TickerSymbol}");

                        // Read
                        var retrievedTicker = context.Tickers.FirstOrDefault(t => t.TickerSymbol == tickerSymbol);
                        if (retrievedTicker != null)
                        {
                            Console.WriteLine($"Ticker found: {retrievedTicker.TickerSymbol}");
                        }

                        // Update
                        if (retrievedTicker != null)
                        {
                            retrievedTicker.TickerSymbol = "UpdatedSymbol";
                            context.SaveChanges();
                            Console.WriteLine($"Ticker updated: {retrievedTicker.TickerSymbol}");
                        }

                        // Delete
                        if (retrievedTicker != null)
                        {
                            context.Tickers.Remove(retrievedTicker);
                            context.SaveChanges();
                            Console.WriteLine($"Ticker deleted: {retrievedTicker.TickerSymbol}");
                        }

                        // Example: Retrieve Price (assuming TickerId is needed for Price)
                        var price = context.Prices.FirstOrDefault(p => p.TickerId == (retrievedTicker != null ? retrievedTicker.Id : 0));
                        if (price != null)
                        {
                            Console.WriteLine($"Price for {retrievedTicker.TickerSymbol}: {price.Price}");
                            await stream.WriteAsync(Encoding.UTF8.GetBytes(Convert.ToString(price.Price)));
                        }
                        else
                        {
                            Console.WriteLine($"No price found for {retrievedTicker?.TickerSymbol}");
                        }
                    }
                }
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }


}
