using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PortsAuthData
{
    public int serverPort { get; set; }
    public int clientPort { get; set; }
}

[Serializable]
public class GameData
{
    public List<PlayerData> PlayerDatas { get; set; }

    public GameData(List<PlayerData> playerDatas)
    {
        PlayerDatas = playerDatas;
    }

    public GameData()
    {
        PlayerDatas = new List<PlayerData>();
    }
}

[Serializable]
public class PlayerData
{
    public string Guid { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public PlayerData(string guid, float posX, float posY, float posZ)
    {
        Guid = guid;
        PosX = posX;
        PosY = posY;
        PosZ = posZ;
    }

    public PlayerData()
    {
    }
}