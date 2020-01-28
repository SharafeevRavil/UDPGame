using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public string Name = "Petya";
    public string Url = "127.0.0.1";

    public float speed = 5f;
    public Test Test;
    
    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var translation = Time.deltaTime * speed * new Vector2(horizontal, vertical);
        
        transform.Translate(translation);

        Test.position = transform.position;
        
        
        StartCoroutine(SendData(translation.magnitude));
    }

    private IEnumerator SendData(object dist)
    {
        WWWForm form = new WWWForm();
        
        form.AddField("userName", Name);
        form.AddField("addValue", ((float)dist).ToString());
        form.AddField("characteristicName", "Distance");
 
        UnityWebRequest www = UnityWebRequest.Post(Url, form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();
    }
    
}
