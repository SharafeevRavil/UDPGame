using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ServerGameData
{
    public List<CreatureData> CreatureDatas;

    public List<ProjectileData> ProjectileDatas;

    public ServerGameData(List<CreatureData> creatureDatas)
    {
        CreatureDatas = creatureDatas;
    }

    public ServerGameData()
    {
        CreatureDatas = new List<CreatureData>();
    }
}