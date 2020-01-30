using System;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib.Utils;
using UDPGameServerLibrary;
using UnityEngine;

public class TestCllient : MonoBehaviour
{
    private Client _client;

    // Start is called before the first frame update
    void Start()
    {
        _client = new Client();
        NetPacketProcessor netPacketProcessor = new NetPacketProcessor();
        _client.Start(netPacketProcessor);
        _client.Connect("127.0.0.1", 666);
    }
}