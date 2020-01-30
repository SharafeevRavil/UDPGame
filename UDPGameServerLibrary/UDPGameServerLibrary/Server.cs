using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UDPGameServerLibrary
{
    public class Server : NetworkWorker
    {
        public void Start(int port, NetPacketProcessor packetProcessor)
        {
            base.Start(packetProcessor);
            
            Listener.ConnectionRequestEvent += OnConnectionRequest;
            NetManager.Start(port);
        }
        
        public void OnConnectionRequest(ConnectionRequest request)
        {
            //Тут по идее надо получать информацию о сборке клиента и сборке сервера, чтобы не допускать разных версий
            //но это видимо не возможно по причине полного доступа юзера к клиенту и любым алгоритмам хэширования
            request.AcceptIfKey("ServerConnectionKey");
        }
    }
}