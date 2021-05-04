using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script permettant de garder en vie les objets désirés.
// Sert lorsqu'il y'a un changement de scène
public class KeepAliveObject : MonoBehaviour
{
    public string username;
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
