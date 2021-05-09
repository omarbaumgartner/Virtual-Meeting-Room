using UnityEngine;
// Adding 
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

// Script permettant de gérer toutes les méthodes reliées au réseau
// Notamment la connexion, déconnexion, création de salle...
public class NetworkManager : MonoBehaviourPunCallbacks // Permet de savoir quand il y'a connexion/deconnexion...
{
    private GameObject Interface;
    private FixedUI InterfaceScript;
    private userlistManager usersPannelScript;
    private KeepAliveObject keepAliveScript;
    private Vector3 DiapoBoardPosition = new Vector3(31.79f, 13.39f, 21.87f);


    // Start is called before the first frame update
    void Start()
    {
        Interface = GameObject.Find("FixedUI");
        InterfaceScript = Interface.GetComponent<FixedUI>();
        keepAliveScript = GameObject.Find("KeepAliveEnvironement").GetComponent<KeepAliveObject>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Server connection
    public void connectToServer()
    {
        if (InterfaceScript.ButtonConnectText.text == "Connect")
        {
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.SerializationProtocolType = ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
            PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = false;
            PhotonNetwork.PhotonServerSettings.AppSettings.Server = InterfaceScript.ServerInput.text;
            Debug.Log("Connecting to " + PhotonNetwork.PhotonServerSettings.AppSettings.Server);
            PhotonNetwork.PhotonServerSettings.AppSettings.Port = 5055;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = null;
            
            // Attribution du username et génération+attribution manuelle d'un ID unique
            PhotonNetwork.LocalPlayer.NickName = keepAliveScript.username;
            Guid uniqueId = Guid.NewGuid();
            AuthenticationValues authValues = new AuthenticationValues();
            authValues.AuthType = CustomAuthenticationType.Custom;
            authValues.UserId = uniqueId.ToString();            
            PhotonNetwork.AuthValues = authValues;

            // Connexion
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.Disconnect();
        }
    }

    // Room creation
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            {
                string roomNumber = InterfaceScript.RoomInput.text;
                if (roomNumber == "")
                {
                    InterfaceScript.errorStatus("A room name is required");
                }
                else
                {
                    Debug.Log("Creating Room with name : " + roomNumber);
                    //InterfaceScript.errorStatus("");
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 10;
                    roomOptions.IsVisible = true;
                    roomOptions.IsOpen = true;
                    roomOptions.PublishUserId = true;
                    //PhotonNetwork.LoadLevel(1);
                    PhotonNetwork.JoinOrCreateRoom("1", roomOptions, TypedLobby.Default);
                    //PhotonNetwork.JoinOrCreateRoom(roomNumber, roomOptions, TypedLobby.Default);
                }
            }
        }
        else
        {
            InterfaceScript.errorStatus("You need to connect to the server first");
        }
    }

    // Join room
    public void JoinOrCreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (!PhotonNetwork.InRoom)
            {
                string roomNumber = InterfaceScript.RoomInput.text;
                if (roomNumber == "")
                {
                    InterfaceScript.errorStatus("A room number is required");
                }
                else
                {
                    InterfaceScript.errorStatus("");
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 10;
                    roomOptions.IsVisible = true;
                    roomOptions.IsOpen = true;
                    roomOptions.PublishUserId = true;
                    PhotonNetwork.JoinOrCreateRoom(roomNumber,roomOptions,TypedLobby.Default);
                }
            }
            else
            {
                Debug.Log("Leaving Room");
                LeaveRoom();
            }
        }
        else
        {
            InterfaceScript.errorStatus("You need to connect to the server first");
        }

    }

    // Leave room
    public void LeaveRoom()
    {
        Debug.Log("Leaving Room");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        base.OnConnectedToMaster();
        // Rejoindre room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
    }

    // Déclencheur une fois la connexion au serveur est établie
    public override void OnConnected()
    {
        Debug.Log("Connected to Server");
        if (InterfaceScript.ServerStatus)
        {
            InterfaceScript.ServerStatus.GetComponent<Text>().color = Color.green;
            InterfaceScript.ServerStatus.GetComponent<Text>().text = "Connected";
        }

        if (InterfaceScript.ButtonConnect)
        {
            InterfaceScript.ButtonConnectText.text = "Disconnect";
        }
        base.OnConnected();
    }

    // Déclencheur lors de la déconnexion du serveur
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Server");
        InterfaceScript.serverStatusText.color = Color.red;
        InterfaceScript.serverStatusText.text = "Disconnected";
        InterfaceScript.ButtonConnectText.text = "Connect";
        Debug.Log(cause);
        base.OnDisconnected(cause);
    }

    // Déclencheur lorsqu'on rejoint une salle
    public override void OnJoinedRoom()
    {
        // Initialiser la liste des utilisateurs déjà présents
        base.OnJoinedRoom();
        Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
        InterfaceScript.usersPannelScript.initList(players);
        InterfaceScript.RoomActionText.text = "Leave";
        InterfaceScript.ActualRoom.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
        //Debug.Log("Number of players in room : " + PhotonNetwork.CurrentRoom.Players.Count);
        //Debug.Log("Joined room : " + PhotonNetwork.CurrentRoom.Name);
        //PhotonNetwork.LoadLevel(1);
    }

    // Déclencheur lorsqu'on quitte une salle
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        InterfaceScript.RoomActionText.text = "Join";
        InterfaceScript.ActualRoom.GetComponent<Text>().text = "Lobby";
        InterfaceScript.usersPannelScript.clearList();
        if (InterfaceScript.PresentationButtons != null)
        {
            InterfaceScript.PresentationButtons.SetActive(false);
        }
        //PhotonNetwork.LoadLevel(0);
    }

    // Déclencheur lorsqu'on créé une salle
    private GameObject diapoPrefab;
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        diapoPrefab = PhotonNetwork.Instantiate("DiapoBoard", DiapoBoardPosition, Quaternion.identity);
        diapoPrefab.name = "DiapoBoard";
        if (diapoPrefab.GetComponent<PhotonView>().IsMine)
        {
            diapoPrefab.GetComponent<Draggable>().enabled = true;
        }
        else {
            diapoPrefab.GetComponent<Draggable>().enabled = false;
        }
        Debug.Log("Room created and Board inited");
    }

    // If a player joined the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        InterfaceScript.usersPannelScript.addElement(newPlayer.UserId, newPlayer.NickName, newPlayer.IsMasterClient);
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player joined the room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        InterfaceScript.usersPannelScript.removeElement(otherPlayer.UserId, otherPlayer.NickName);
        if (GameObject.Find("DiapoBoard") == null)
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                diapoPrefab = PhotonNetwork.Instantiate("DiapoBoard", transform.position, Quaternion.identity);
                diapoPrefab.name = "DiapoBoard";
                if (diapoPrefab.GetComponent<PhotonView>().IsMine)
                {
                    diapoPrefab.GetComponent<Draggable>().enabled = true;
                }
                else
                {
                    diapoPrefab.GetComponent<Draggable>().enabled = false;
                }
            }
        }
        base.OnPlayerLeftRoom(otherPlayer);
    }

    // Déclencheur lorsque la création de la salle a échoué
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed creating room");
        InterfaceScript.errorStatus(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed joining room");
        InterfaceScript.errorStatus(message);
    }

}
