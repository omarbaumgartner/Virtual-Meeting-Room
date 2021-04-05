using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAlive : MonoBehaviour
{
    private static KeepAlive playerInstance;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
