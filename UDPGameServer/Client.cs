using System;
using System.Net;

namespace UDPGameServer
{
    public class Client
    {
        public int ServerPort { get; private set; }
        public IPEndPoint ClientSendIp { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime LastReceive { get; private set; }
        public bool ReceivedFirstInfo { get; private set; }

        public void Receive()
        {
            ReceivedFirstInfo = true;
            LastReceive = DateTime.Now;
        }

        public Client(Guid guid, IPEndPoint clientSendIp, int serverPort)
        {
            ClientSendIp = clientSendIp;
            ServerPort = serverPort;
            Guid = guid;
            LastReceive = DateTime.Now;
        }
    }
}