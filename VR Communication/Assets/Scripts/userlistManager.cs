using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script permettant la gestion de la liste des utilisateurs affichée dans l'IU
// Fournit aussi les méthodes permettant d'exclure un utilisateur, ou de passer son rôle de maître de salle à un autre utilisateur

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

    // Initialisation de la liste des joueurs
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

    // Ajout d'un utilisateur dans la liste ( lorsqu'il rejoint une salle ) 
    public void addElement(string id, string username)
    {
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
    
    // Suppression d'un utilisateur de la liste ( lorsqu'il quittte la salle )
    public void removeElement(string id, string username)
    {
        GameObject user = GameObject.Find(username + '_' + id);
        if (user != null)
        {
            Destroy(user);
        }
    }

    // Transfert de permissions à un utilisateur
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
                    // To do passer le lead.
                    //PhotonNetwork.SetMasterClient(player.Value);
                    //PhotonView photonView;
                    //photonView.RPC("OnMasterChanged", RpcTarget.All);
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

    // Exclusion d'un utilisateur de la salle
    public void kickUser(string id, string username)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
            foreach (KeyValuePair<int, Player> player in players)
            {
                if (player.Value.UserId == id && player.Value.NickName == username)
                {
                    PhotonNetwork.CloseConnection(player.Value);
                }
            }
        }
        else
        {
            FixedUIScript.errorStatus("You do not have the rights to do that");
        }

    }

    // Suppression de tous les éléménts dans la liste ( lorsque l'utilisateur quitte une salle )
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

