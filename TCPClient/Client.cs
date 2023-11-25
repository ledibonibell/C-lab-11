using System.Net.Sockets;
using System.Net;

using TcpClient client = new TcpClient();

await client.ConnectAsync("127.0.0.127", 8888);
string? tickername = "";

var networkstream = client.GetStream();

using StreamReader streamReader = new StreamReader(networkstream);
using StreamWriter streamWriter = new StreamWriter(networkstream);

while (true)
{
    Console.WriteLine("Write ticker name: ");
    tickername= Console.ReadLine();
    if (tickername != "stop")
    {
        await streamWriter.WriteLineAsync(tickername);
        await streamWriter.FlushAsync();
        string? ans = await streamReader.ReadLineAsync();
        if (ans != "incorrect ticker")
        {
            Console.WriteLine($"Last price of ticker {tickername}: {ans}$");
        }
        else
        {
            Console.WriteLine(ans);
        }
        tickername = "";
    }
    else if (tickername == "stop")
    {
        await streamWriter.WriteLineAsync(tickername);
        await streamWriter.FlushAsync();
        break;
    }
}

