using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public float speed = 5f;
    public Test Test;

    public MyGameManager myGameManager;

    // Update is called once per frame
    void Update()
    {
        if (myGameManager.IsPlayerDead)
        {
            gameObject.SetActive(false);
        }

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector2(horizontal, vertical) * speed * Time.deltaTime);

        /*lock (Test.PlayerLock)
        {
            Test.position = transform.position;
        }*/
    }
}