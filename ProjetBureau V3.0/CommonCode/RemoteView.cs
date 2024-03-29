﻿using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CommonCode
{
    /// <summary>
    ///  Classe RemoteView permettant la connexion a une application console déportée ayant accès a l'état d'avancement de la sauvegarde 
    ///  RemoteView class allowing connection to a remote console application with access to the progress of the backup 
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
            Task.Run(() =>
            {
                while (true)
                {
                    var client = listenSocket.Accept();
                    lock (clients)
                    {
                        clients.Add(client);
                    }
                }
            });
        }

        /// <summary>
        /// Mise en forme des informations de la sauvegarde en temps réel (Pourcentage | Nom du fichier | Nombre de fichier restant)
        /// Format real-time backup information (Percentage | File's name | Number of remaining files)
        /// </summary>
        public void ControlProgress(string fileFullName, int countfile, int totalFileToCopy, double percentage)
        {
            string fileName = System.IO.Path.GetFileName(fileFullName);
            SendText($"{percentage}% | {countfile}/{totalFileToCopy} {Traduction.Instance.Langue.InCopy} | {fileName}");
        }

        /// <summary>
        /// Information de début et de fin de sauvegarde
        /// Backup start and end information
        /// </summary>
        public void Progress(bool state)
        {
            SendText(!state ? Traduction.Instance.Langue.Buffering : Traduction.Instance.Langue.Complete);
        }

        /// <summary>
        /// Envoie des données a l'application déportée
        /// Send data to the remote application
        /// </summary>
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