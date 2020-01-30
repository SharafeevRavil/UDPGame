using LiteNetLib;
using LiteNetLib.Utils;

namespace UDPGameServerLibrary
{
    public abstract class NetworkWorker
    {
        protected EventBasedNetListener Listener;
        protected NetManager NetManager;
        protected NetPacketProcessor PacketProcessor;
        protected NetDataWriter Writer;

        protected void Start(NetPacketProcessor packetProcessor)
        {
            Listener = new EventBasedNetListener();
            NetManager = new NetManager(Listener);
            Writer = new NetDataWriter();

            PacketProcessor = packetProcessor;

            Listener.NetworkReceiveEvent += ListenerOnNetworkReceiveEvent;
        }

        protected void SendPacketSerializable<T>(NetPeer netPeer, T packet, DeliveryMethod deliveryMethod)
            where T : INetSerializable
        {
            Writer.Reset();
            packet.Serialize(Writer);
            netPeer.Send(Writer, deliveryMethod);
        }

        private void ListenerOnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            PacketProcessor.ReadAllPackets(reader);
        }
    }
}