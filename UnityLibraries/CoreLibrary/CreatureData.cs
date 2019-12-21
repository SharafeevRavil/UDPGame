using System;

[Serializable]
public class CreatureData
{
    public string Guid;
    public float PosX;
    public float PosY;
    public float PosZ;

    public float Health;

    public float Speed;
    
    public float R;
    public float G;
    public float B;

    public bool IsDead;
    
    public CreatureData(string guid, float posX, float posY, float posZ)
    {
        Guid = guid;
        PosX = posX;
        PosY = posY;
        PosZ = posZ;
    }

    public CreatureData()
    {
    }
}