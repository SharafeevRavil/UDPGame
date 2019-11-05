using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UDPGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();

            threads.Add(new Thread(ListenConnections));
            threads.Add(new Thread(MarchClients));
            threads.Add(new Thread(TCPAuth));

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.Read();
        }

        private static readonly HashSet<IPEndPoint> ClientsIPs = new HashSet<IPEndPoint>();
        private static readonly object ClientsIPsLock = new object();

        private static readonly GameData GameData = new GameData();
        private static readonly object GameDataLock = new object();

        static void TCPAuth()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8001);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                var stream = client.GetStream();

                IPEndPoint ipEndPoint = (IPEndPoint) client.Client.RemoteEndPoint;
                lock (ClientsIPsLock)
                {
                    ClientsIPs.Add(ipEndPoint);
                }

                Console.WriteLine($"Зарегистрирован клиент {ipEndPoint.Address}:{ipEndPoint.Port}");
                stream.Write(Encoding.UTF8.GetBytes(ipEndPoint.Port.ToString()));


                stream.Close();
                client.Close();
            }
        }

        static void ListenConnections()
        {
            UdpClient client = new UdpClient(8001);

            IPEndPoint ip = null;

            var data = client.Receive(ref ip);
            Console.WriteLine($"{DateTime.Now} пришел пакет");
            client.Close();
            new Thread(ListenConnections).Start();

            //game data
            string json = Encoding.UTF8.GetString(data);
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            lock (GameDataLock)
            {
                if (GameData.PlayerDatas.Any(x => x.Guid == playerData.Guid))
                {
                    GameData.PlayerDatas.Remove(GameData.PlayerDatas.First(x => x.Guid == playerData.Guid));
                }

                GameData.PlayerDatas.Add(playerData);
            }
        }

        static void MarchClients()
        {
            while (true)
            {
                lock (ClientsIPsLock)
                {
                    foreach (var clientIP in ClientsIPs)
                    {
                        new Thread(() => SendClient(clientIP)).Start();
                    }
                }

                Thread.Sleep(200);
            }
        }

        static void SendClient(IPEndPoint clientIP)
        {
            UdpClient client = new UdpClient();
            client.Connect(clientIP);
            string json;
            lock (GameDataLock)
            {
                json = JsonConvert.SerializeObject(GameData);
            }

            byte[] data = Encoding.UTF8.GetBytes(json);
            Console.WriteLine($"{DateTime.Now} перед посылкой на {clientIP}");
            client.Send(data, data.Length);
            Console.WriteLine($"{DateTime.Now} после посылки на {clientIP}");
            client.Close();
        }
    }
}