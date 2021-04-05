using UnityEngine;
// Adding 
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks // Permet de savoir quand il y'a connexion/deconnexion...
{
    private GameObject Interface;
    private InterfaceCameraFlow InterfaceScript;

    // Start is called before the first frame update
    void Start()
    {
        Interface = GameObject.Find("FixedUI");
        InterfaceScript = Interface.GetComponent<InterfaceCameraFlow>();
        //ActualRoom = GameObject.Find("ActualRoomText");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Server connection
    public void connectToServer()
    {
        Text ButtonConnectText = GameObject.Find("ConnectText").GetComponent<Text>();
        if (ButtonConnectText.text == "Connect")
        {
            InputField InputServer = GameObject.Find("ServerInput").GetComponent<InputField>();
            PhotonNetwork.PhotonServerSettings.AppSettings.Server = InputServer.text;
            Debug.Log("Connecting to " + PhotonNetwork.PhotonServerSettings.AppSettings.Server);
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
                string roomNumber = "1";
                //string roomNumber = GameObject.Find("RoomInputText").GetComponent<Text>().text;
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
                    //PhotonNetwork.LoadLevel(1);
                    PhotonNetwork.JoinOrCreateRoom("RoomA", roomOptions, TypedLobby.Default);
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
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("In Room : " + PhotonNetwork.InRoom);
            if (!PhotonNetwork.InRoom)
            {
                string roomNumber = GameObject.Find("RoomInputText").GetComponent<Text>().text;
                if (roomNumber == "")
                {
                    InterfaceScript.errorStatus("A room name is required");
                }
                else
                {
                    InterfaceScript.errorStatus("");
                    Debug.Log("Creating Room");
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 10;
                    roomOptions.IsVisible = true;
                    roomOptions.IsOpen = true;
                    PhotonNetwork.JoinRoom(roomNumber);
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
        //CreateRoom();
        //Debug.Log("Connecting to " + PhotonNetwork.ServerAddress); 
        //PhotonNetwork.LoadLevel(1);
        //PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    // Déclencheur une fois la connexion au serveur est établie
    public override void OnConnected()
    {
        Debug.Log("Connected to Server");
        GameObject serverStatus = GameObject.Find("DisconnectedText");
        if (serverStatus)
        {
            Text serverStatusText = serverStatus.GetComponent<Text>();
            serverStatusText.color = Color.green;
            serverStatusText.text = "Connected";
        }

        GameObject ButtonConnect = GameObject.Find("ConnectText");
        if (ButtonConnect)
        {
            Text ButtonConnectText = ButtonConnect.GetComponent<Text>();
            ButtonConnectText.text = "Disconnect";
        }
        base.OnConnected();
    }

    // Déclencheur lors de la déconnexion du serveur
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Server");
        //Text serverStatus = GameObject.Find("DisconnectedText").GetComponent<Text>();
        //serverStatus.color = Color.red;
        //serverStatus.text = "Disconnected";
        //Text ButtonConnectText = GameObject.Find("ConnectText").GetComponent<Text>();
        //ButtonConnectText.text = "Connect";
        base.OnDisconnected(cause);
    }

    // Déclencheur lorsqu'on rejoint une salle
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //PhotonNetwork.LoadLevel(1);
        //GameObject.Find("JoinRoomText").GetComponent<Text>().text = "Leave Room";
        Debug.Log("Joined room : " + PhotonNetwork.CurrentRoom.Name);
        //ActualRoom.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
    }

    // Déclencheur lorsqu'on créé une salle
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created");
    }

    // Déclencheur lorsqu'on quitte une salle
    public override void OnLeftRoom()
    {
        //GameObject.Find("CreateRoomText").GetComponent<Text>().text = "Create Room";
        //ActualRoom.GetComponent<Text>().text = "Lobby";
        base.OnLeftRoom();
        //PhotonNetwork.LoadLevel(0);
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


    // If a player joined the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player joined the room");
    }
}
