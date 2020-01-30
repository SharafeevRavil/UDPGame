using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UDPGameServerLibrary
{
    public class Client : NetworkWorker
    {
        public new void Start(NetPacketProcessor packetProcessor)
        {
            base.Start(packetProcessor);
            
            NetManager.Start();
        }

        public void Connect(string ip, int port)
        {
            NetManager.Connect(ip, port, "ServerConnectionKey");
        }
    }
}