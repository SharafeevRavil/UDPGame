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
    private int port;
    
    void Start()
    {
        guid = Guid.NewGuid().ToString();
        port = UnityEngine.Random.Range(0, 65536);

        UdpClient client = new UdpClient();
        client.Connect(hostname, 8001);
        string message = port.ToString();
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data, data.Length);
        client.Close();


        _receiveThread = new Thread(ReceiveData);
        _receiveThread.IsBackground = true;
        _receiveThread.Start();

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
            client.Connect(hostname, 8001);
            string json = JsonUtility.ToJson(playerData);
            byte[] data = Encoding.UTF8.GetBytes(json);
            client.Send(data, data.Length);
            client.Close();

            Thread.Sleep(200);
        }
    }

    void ReceiveData()
    {
        UdpClient client = new UdpClient(port);
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