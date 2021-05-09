using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkChairManager : MonoBehaviour
{

    GameObject[] chairs;
    // Start is called before the first frame update
    void Start()
    {
        chairs = GameObject.FindGameObjectsWithTag("Chair");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sitInChair() {
        Debug.Log("Looking for free chair");
        GameObject DiapoBoard = GameObject.Find("DiapoBoard");
        GameObject Player = GameObject.Find("ViveCameraRig");
        GameObject Chair = GameObject.Find("TheaterChair1");
        // Player go to position of chair and freeze position ( not rotation )
        //Player.transform.LookAt(DiapoBoard.transform.position, transform.up);
        Player.transform.position = Chair.transform.position + new Vector3(-0.03f,-1f,0.2f);
        GameObject.Find("ViveCurvePointers").transform.position = Chair.transform.position + new Vector3(-0.03f, -1f, 0.2f);
        GameObject.Find("VivePointers").transform.position = Chair.transform.position + new Vector3(-0.03f, -1f, 0.2f);


        //Player.transform.rotation = Quaternion.Euler(0, 170f, 0);

        Debug.Log("Chair"+Chair.transform.position);
        Debug.Log("Player"+Player.transform.position);
        return;
        
        
        
        
        
        /*
        foreach (GameObject chair in chairs)
        {
            // Check free chair
            if (chair.GetComponent<chairState>().isTaken == false)
            {
                //GameObject Player = GameObject.FindGameObjectWithTag("isMine");
                // Player go to position of chair and freeze position ( not rotation )
                Player.transform.position = chair.transform.position;                
                // Freeze player TODO
                Debug.Log("Found chair at : " + chair.transform.position);
                return;
            }
        }
        Debug.Log("There's no available chair");*/
    }

    public void freeChair(GameObject Player) { 
        // remove concerned pair ( include RPC ) 

        // Teleport player to enter of room
    }

    void updateChairStatus() { 
    
    }


}
