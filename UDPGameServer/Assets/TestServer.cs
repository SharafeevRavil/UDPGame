using System;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib.Utils;
using UDPGameServerLibrary;
using UnityEngine;

public class TestServer : MonoBehaviour
{
    private Server _server;

    public void Start()
    {
        _server = new Server();
        NetPacketProcessor packetProcessor = new NetPacketProcessor();
        _server.Start(666, packetProcessor);
    }
}
