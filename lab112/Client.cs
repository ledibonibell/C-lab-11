using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Жестко закодированный тикер (замените на ваш желаемый тикер)
            var input = "AAPL";

            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", 8888);

            var stream = tcpClient.GetStream();
            try
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes(input));
                byte[] response = new byte[1024];
                await stream.ReadAsync(response);
                var price = Encoding.UTF8.GetString(response);
                Console.WriteLine($"Price for {input} is {price}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }


}
