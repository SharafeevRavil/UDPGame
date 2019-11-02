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
        static async Task Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();

            threads.Add(new Thread(ListenConnections));
            threads.Add(new Thread(MarchClients));

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.Read();
        }

        private static HashSet<IPEndPoint> clientsIPs = new HashSet<IPEndPoint>();

        private static GameData _gameData = new GameData();

        static void ListenConnections()
        {
            UdpClient client = new UdpClient(8001);
            while (true)
            {
                IPEndPoint ip = null;
                var data = client.Receive(ref ip);
                string message = Encoding.UTF8.GetString(data);
                int port = 0;
                if (int.TryParse(message, out port))
                {
                    ip.Port = port;
                    clientsIPs.Add(ip);
                }
                else
                {
                    //game data
                    string json = Encoding.UTF8.GetString(data);
                    PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);
                    if (_gameData.PlayerDatas.Any(x => x.Guid == playerData.Guid))
                    {
                        _gameData.PlayerDatas.Remove(_gameData.PlayerDatas.First(x => x.Guid == playerData.Guid));
                    }

                    _gameData.PlayerDatas.Add(playerData);
                }
            }
        }

        static void MarchClients()
        {
            while (true)
            {
                foreach (var clientIP in clientsIPs)
                {
                    UdpClient client = new UdpClient();
                    client.Connect(clientIP);
                    string json = JsonConvert.SerializeObject(_gameData);
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    client.Send(data, data.Length);
                    client.Close();
                }

                Thread.Sleep(200);
            }
        }
    }
}