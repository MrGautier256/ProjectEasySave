using System.Net;
using System.Net.Sockets;
using System.Text;
public partial class Program
{
    static void Main()
    {
        string text;
        IPEndPoint ipep = new(IPAddress.Parse("127.0.0.1"), 9050);
        Socket listenSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listenSocket.Connect(ipep);
        }
        catch (Exception)
        {
            Console.WriteLine("Server unreachable");
            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
            Environment.Exit(1);
        }

        int byteReveived = 1;

        try
        {
            while (byteReveived > 0)
            {
                byte[] data = new byte[1024];
                byteReveived = listenSocket.Receive(data);
                text = Encoding.UTF8.GetString(data, 0, byteReveived);
                Console.WriteLine(text+"\n");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Connexion Interrompue");
        }
        Console.WriteLine("\nPress any key to close...");
        Console.ReadKey();
    }

}