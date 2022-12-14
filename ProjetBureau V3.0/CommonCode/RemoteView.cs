using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CommonCode
{
    /// <summary>
    ///  Classe Controller
    ///  Class Controller
    /// </summary>

    public class RemoteView : IViewProgress
    {
        private List<Socket> clients = new();
        public RemoteView()
        {
            IPEndPoint ipep = new(IPAddress.Parse("127.0.0.1"), 9050);
            Socket listenSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(ipep);
            listenSocket.Listen();
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        var client = listenSocket.Accept();
            //        lock (clients)
            //        {
            //            clients.Add(client);
            //        }
            //    }
            //});
        }
        public void ControlProgress(string fileName, int countfile, int totalFileToCopy, double percentage)
        {
            SendText($"{percentage}% | {countfile}/{totalFileToCopy} {Traduction.Instance.Langue.InCopy} | {fileName}");
        }

        public void Progress(bool state)
        {
            SendText(!state ? Traduction.Instance.Langue.Buffering : Traduction.Instance.Langue.Complete);
        }

        public void SendText(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            List<Socket> clientsCopy;
            lock (clients)
            {
                clientsCopy = clients.ToList();
            }

            foreach (var client in clientsCopy)
            {
                try
                {
                    client.Send(data, data.Length, SocketFlags.None);
                }
                catch (SocketException)
                {
                    lock (clients)
                    {
                        clients.Remove(client);
                    }
                }
            }
        }
    }
}