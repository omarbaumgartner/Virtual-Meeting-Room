using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userlistManager : MonoBehaviour
{
    public GameObject grid;
    public GameObject userPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    void addElement(int id,string username)
    {
        userPrefab.transform.parent = grid.transform;
        userPrefab.name = username+'_'+id;
        userPrefab.transform.Find("UsernameText").GetComponent<Text>().text = username;
        Instantiate(userPrefab);
    }
    void removeElement(int id, string username)
    {
        GameObject user = GameObject.Find(username + '_' + id);
        if (user != null)
        {
            Destroy(user);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

