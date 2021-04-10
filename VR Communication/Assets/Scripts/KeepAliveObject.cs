using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAliveObject : MonoBehaviour
{
    private static KeepAliveObject playerInstance;
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
            Object.Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
