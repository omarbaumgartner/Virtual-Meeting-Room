using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkChairManager : MonoBehaviour
{

    GameObject[] chairs;
    Dictionary<GameObject, GameObject> pairs;
    // Start is called before the first frame update
    void Start()
    {
        chairs = GameObject.FindGameObjectsWithTag("Chair");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void sitInChair(GameObject Player) { 
        // Check free chair

        // Free Chair ?
        // Player go to position of chair and freeze position ( not rotation )
        // add player,chair to pairs ( include RPC )
        // No Free chairs ?

    }

    void freeChair(GameObject Player) { 
        // remove concerned pair ( include RPC ) 

        // Teleport player to enter of room
    }


}
