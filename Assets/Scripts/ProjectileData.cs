using System;

[Serializable]
public class ProjectileData
{
    public string Guid;
    public bool IsEnemy;
    public float X;
    public float Y;
    public float Z;

    public float XDirection;
    public float YDirection;
    public float ZDirection;

    public float Velocity;

    public float Damage;

    public float R;
    public float G;
    public float B;

    public bool IsDead;
}