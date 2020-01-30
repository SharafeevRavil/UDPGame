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
    public class Program
    {
        
        delegate void TestDelegate();
        
        public static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();

            threads.Add(new Thread(TcpAuth));

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.Read();
        }

        private static readonly Dictionary<Guid, Client> Clients = new Dictionary<Guid, Client>();
        private static readonly object ClientsLock = new object();

        private static readonly GameData GameData = new GameData();
        private static readonly object GameDataLock = new object();

        private static void TcpAuth()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8001);
            listener.Start();

            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                var stream = tcpClient.GetStream();

                StringBuilder guidResponse = new StringBuilder();
                if (stream.CanRead)
                {
                    byte[] myReadBuffer = new byte[1024];
                    do
                    {
                        var numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                        guidResponse.AppendFormat("{0}",
                            Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                    } while (stream.DataAvailable);
                }

                Guid clientGuid = Guid.Parse(guidResponse.ToString());

                //Рандомлю порты
                Random random = new Random();
                int clientPort = random.Next(32768, 65535);
                int serverPort = random.Next(32768, 65535);
                lock (ClientsLock)
                {
                    while (Clients.Values.Any(x => x.ServerPort == serverPort))
                    {
                        serverPort = random.Next(32768, 65535);
                    }
                }


                IPEndPoint clientSendIp = (IPEndPoint) tcpClient.Client.RemoteEndPoint;
                clientSendIp.Port = clientPort;
                Client client = new Client(clientGuid, clientSendIp, serverPort);
                lock (ClientsLock)
                {
                    Clients[clientGuid] = client;
                }


                new Thread(SendClient).Start(client);
                new Thread(ListenClient).Start(client);


                Console.WriteLine($"Зарегистрирован клиент {clientSendIp.Address}:{clientSendIp.Port}");
                //Пишу в ответ порты
                stream.Write(
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new PortsAuthData(clientPort, serverPort))));

                stream.Close();
                tcpClient.Close();
            }
        }

        static void ListenClient(object clientObj)
        {
            Client client = (Client) clientObj;

            while (true)
            {
                lock (ClientsLock)
                {
                    if (!Clients.ContainsKey(client.Guid))
                    {
                        //Disconnected, not need to listen
                        return;
                    }
                }

                UdpClient udpClient = new UdpClient(client.ServerPort);

                IPEndPoint ip = null;

                var data = udpClient.Receive(ref ip);
                Console.WriteLine($"{DateTime.Now} пришел пакет от {ip}");
                udpClient.Close();

                client.Receive();

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
        }

        private static void SendClient(object clientObj)
        {
            Client client = (Client) clientObj;
            IPEndPoint clientIp = client.ClientSendIp;

            while (true)
            {
                if (!client.ReceivedFirstInfo)
                {
                    Thread.Sleep(200);
                    continue;
                }

                if (DateTime.Now - client.LastReceive > TimeSpan.FromSeconds(5))
                {
                    Disconnect(client);
                    break;
                }

                UdpClient udpClient = new UdpClient();
                udpClient.Connect(clientIp);
                string json;
                lock (GameDataLock)
                {
                    json = JsonConvert.SerializeObject(GameData);
                }

                byte[] data = Encoding.UTF8.GetBytes(json);
                Console.WriteLine($"{DateTime.Now} перед посылкой на {clientIp}");
                udpClient.Send(data, data.Length);
                Console.WriteLine($"{DateTime.Now} после посылки на {clientIp}");
                udpClient.Close();

                Thread.Sleep(200);
            }
        }

        private static void Disconnect(Client client)
        {
            //disconnect
            lock (ClientsLock)
            {
                Clients.Remove(client.Guid);
            }

            lock (GameDataLock)
            {
                var disconnectedPlayerData = GameData.PlayerDatas.Find(data => data.Guid == client.Guid.ToString());
                GameData.PlayerDatas.Remove(disconnectedPlayerData);
            }
        }
    }
}