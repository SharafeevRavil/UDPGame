using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviour : MonoBehaviour
{
    public ProjectileData Data;

    public void UpdateData(ProjectileData newData)
    {
        Data = newData;

        transform.position = new Vector3(Data.X, Data.Y, Data.Z);
    }

    public ProjectileBehaviour(ProjectileData data)
    {
        UpdateData(data);
    }

    private void Update()
    {
        Vector3 direction = new Vector3(Data.XDirection, Data.YDirection, Data.ZDirection).normalized;
        transform.TransformDirection(Time.deltaTime * Data.Velocity * direction);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}