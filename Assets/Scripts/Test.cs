using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class Test : MonoBehaviour
{
    public string hostname;

    private Thread _receiveThread;
    private Thread _sendThread;

    private readonly object _lockData = new object();
    private GameData _gameData;
    private bool _dataChanged = false;

    public string guid;
    private PortsAuthData _portsAuthData;

    void Start()
    {
        guid = Guid.NewGuid().ToString();
        
        TcpClient client = new TcpClient();
        client.Connect(hostname, 8001);
        StringBuilder portsResponse = new StringBuilder();
        NetworkStream stream = client.GetStream();
        //Send guids
        byte[] buffer = Encoding.UTF8.GetBytes(guid);
        stream.Write(buffer, 0, buffer.Length);
        //Receive ports from server
        if (stream.CanRead)
        {
            byte[] myReadBuffer = new byte[1024];
            do
            {
                var numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                portsResponse.AppendFormat("{0}",
                    Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
            } while (stream.DataAvailable);
        }
        _portsAuthData = JsonUtility.FromJson<PortsAuthData>(portsResponse.ToString());
        //Start a thread, that receives packages
        _receiveThread = new Thread(ReceiveData);
        _receiveThread.IsBackground = true;
        _receiveThread.Start();
        //Start a thread, that send packages
        _sendThread = new Thread(SendData);
        _sendThread.IsBackground = true;
        _sendThread.Start();
    }

    private void Update()
    {
        lock (_lockData)
        {
            if (_dataChanged)
            {
                _dataChanged = false;
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                foreach (var playerData in _gameData.PlayerDatas.Where(x => x.Guid != guid))
                {
                    Instantiate(prefab, new Vector3(playerData.PosX, playerData.PosY, playerData.PosZ),
                        Quaternion.identity, transform);
                }
            }
        }
    }

    public Vector3 position;
    public readonly object PositionLock = new object();

    void SendData()
    {
        while (true)
        {
            PlayerData playerData;
            lock (PositionLock)
            {
                playerData = new PlayerData(guid, position.x, position.y, position.z);
            }

            UdpClient client = new UdpClient();
            client.Connect(hostname, _portsAuthData.serverPort);
            string json = JsonUtility.ToJson(playerData);
            byte[] data = Encoding.UTF8.GetBytes(json);
            client.Send(data, data.Length);
            client.Close();

            Thread.Sleep(200);
        }
    }

    void ReceiveData()
    {
        UdpClient client = new UdpClient(_portsAuthData.clientPort);
        while (true)
        {
            IPEndPoint ip = null;
            byte[] data = client.Receive(ref ip);
            lock (_lockData)
            {
                string json = Encoding.UTF8.GetString(data);
                _gameData = JsonUtility.FromJson<GameData>(json);
                _dataChanged = true;
                Debug.Log(json);
            }
        }
    }

    public GameObject prefab;

    private void OnDestroy()
    {
        if (_receiveThread.IsAlive)
        {
            _receiveThread.Abort();
        }

        if (_sendThread.IsAlive)
        {
            _sendThread.Abort();
        }
    }
}