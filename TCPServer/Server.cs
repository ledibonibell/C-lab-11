using System.Net;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using TCPServer;
using TicketsLibrary;

var ip = IPAddress.Parse("127.0.0.127");

var server = new TcpListener(ip, 8888);

try
{
    server.Start();
    Console.WriteLine("Server start");

    while (true)
    {
        var client = await server.AcceptTcpClientAsync();
        Console.WriteLine("client connected");
        Task.Run(async () => await TickerCondition(client));
    }
}
finally
{
    server.Stop();
}


async Task TickerCondition(TcpClient client)
{

    var networkstream = client.GetStream();
    using StreamReader streamReader = new StreamReader(networkstream);
    using StreamWriter streamWriter = new StreamWriter(networkstream);
    while (true)
    {
        var tickername = await streamReader.ReadLineAsync();
        if (tickername == "stop")
        {
            break;
        }
        Console.WriteLine($"ticker: {tickername}");
        using (ApplicationContext db = new ApplicationContext())
        {
            Ticker? ticker = db.Tickers.FirstOrDefault(p => p.name == tickername);
            string? ans;
            if (ticker != null)
            {
                db.Prices.Where(u => u.tickerName == ticker.name).Load();
                Price? price = ticker.Prices.Last();
                ans = price.price.ToString();
            }
            else
            {
                ans = "incorrect ticker";
            }

            await streamWriter.WriteLineAsync(ans);
            await streamWriter.FlushAsync();
        }
    }
    client.Close();
}