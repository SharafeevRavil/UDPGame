using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public Test Test;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = transform.position - new Vector3(Screen.width, Screen.height) / 2 + Input.mousePosition;
            Vector3 playerPosition = transform.position;
            Vector3 direction = position - playerPosition;

            /*lock (Test.PlayerLock)
            {
                Test.ProjectileDatas.Add(new ProjectileData()
                {
                    X = playerPosition.x,
                    Y = playerPosition.y,
                    Z = playerPosition.z,
                    XDirection = direction.x,
                    YDirection = direction.y,
                    ZDirection = direction.z,
                    Velocity = 10
                });
            }*/
        }
    }
}