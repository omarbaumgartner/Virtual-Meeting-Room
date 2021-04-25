using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userlistManager : MonoBehaviour
{
    public GameObject grid;
    public GameObject userPrefab;
    public FixedUI FixedUIScript;
    // Start is called before the first frame update
    void Start()
    {
        FixedUIScript = GameObject.Find("FixedUI").GetComponent<FixedUI>();
    }

    public void initList(Dictionary<int, Player> usersList)
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<int, Player> player in usersList)
        {
            addElement(player.Value.UserId, player.Value.NickName);
        }
    }

    public void addElement(string id, string username)
    {
        Debug.Log("Adding new element with id :" + id + " and username : " + username);
        GameObject newUserElement = Instantiate(userPrefab, new Vector3(grid.transform.position.x, grid.transform.position.y, grid.transform.position.z), grid.transform.rotation);
        newUserElement.transform.SetParent(grid.transform);
        newUserElement.name = username + '_' + id;
        newUserElement.transform.localScale = new Vector3(1, 1, 1);
        newUserElement.transform.Find("UsernameButton").GetComponentInChildren<Text>().text = username;
        Button kickButton = newUserElement.transform.Find("KickButton").GetComponent<Button>();
        kickButton.onClick.AddListener(() => { 
            kickUser(id,username); 
        });
        Button leadButton = newUserElement.transform.Find("SetMaster").GetComponent<Button>();
        leadButton.onClick.AddListener(() => {
            giveRights(id, username);
        });
    }
    public void removeElement(string id, string username)
    {
        Debug.Log("Removing element with id :" + id + " and username : " + username);
        GameObject user = GameObject.Find(username + '_' + id);
        if (user != null)
        {
            Destroy(user);
        }
    }

    public void giveRights(string id, string username)
    {
        Debug.Log("Giving lead user with username " + username + " and id : " + id);
        if (PhotonNetwork.IsMasterClient)
        {
            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
            foreach (KeyValuePair<int, Player> player in players)
            {
                if (player.Value.UserId == id && player.Value.NickName == username)
                {
                    Debug.Log("Giving rights to user confirmed");
                    PhotonNetwork.SetMasterClient(player.Value);
                    PhotonView photonView;
                    photonView.RPC("OnMasterChanged", RpcTarget.All);

                }
            }
        }
        else
        {
            FixedUIScript.errorStatus("You do not have the rights to do that");
        }
    }

    [PunRPC]
    // S'enclenche chez l'utilisateur qui reçoit le Master
    public void OnMasterChanged()
    {
        // Afficher Diapo
    }

    public void kickUser(string id, string username)
    {
        Debug.Log("Kicking user with username " + username + " and id : " + id);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Is Master");
            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
            foreach (KeyValuePair<int, Player> player in players)
            {
                Debug.Log(player.Value.UserId);
                Debug.Log(id);
                Debug.Log(player.Value.NickName);
                Debug.Log(username);
                if (player.Value.UserId == id && player.Value.NickName == username)
                {
                    Debug.Log("Kicking user confirmed");
                    PhotonNetwork.CloseConnection(player.Value);
                }
            }
        }
        else
        {
            FixedUIScript.errorStatus("You do not have the rights to do that");
        }

    }

    public void clearList()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

