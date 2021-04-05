﻿using HTC.UnityPlugin.Vive;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceCameraFlow : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float distanceFromCamera = 2F;
    private GameObject PlayerUI;
    private GameObject ActualRoom;
    private GameObject ButtonConnect;
    private GameObject ServerStatus;
    private GameObject NetworkManager;
    private NetworkManager networkManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager = GameObject.Find("Network Manager");
        networkManagerScript = NetworkManager.GetComponent<NetworkManager>();
        PlayerUI = GameObject.Find("PlayerUI");
        ActualRoom = GameObject.Find("ActualRoomText");
        ButtonConnect = GameObject.Find("Connect2ServerButton");
        ServerStatus = GameObject.Find("DisconnectedText");
        GameObject.Find("ServerInput").GetComponent<InputField>().text = "172.30.40.34";
        GameObject.Find("RoomInput").GetComponent<InputField>().text = "1";
        PlayerUI.SetActive(false);
        target = playerCamera.transform;
    }

    // Mise à jour des données affichés sur l'interface du joueur lors de son affichage
    public void HasOpenedInterface()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Button ButtonConnectText = ButtonConnect.GetComponent<Button>();
            ButtonConnectText.GetComponentInChildren<Text>().text = "Disconnect";
            Text serverStatusText = ServerStatus.GetComponent<Text>();
            serverStatusText.color = Color.green;
            serverStatusText.text = "Connected";
            if (PhotonNetwork.InRoom)
            {
                ActualRoom.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
                GameObject.Find("JoinRoomText").GetComponent<Text>().text = "Leave Room";
            }
            else
            {
                ActualRoom.GetComponent<Text>().text = "Lobby";
            }
        }
        else
        {
            Button ButtonConnectText = ButtonConnect.GetComponent<Button>();
            ButtonConnectText.GetComponentInChildren<Text>().text = "Connect";
            Text serverStatusText = ServerStatus.GetComponent<Text>();
            serverStatusText.color = Color.red;
            serverStatusText.text = "Disconnected";
            ActualRoom.GetComponent<Text>().text = "Lobby";
        }
    }

    // Clavier pour insérer l'adresse IP du serveur
    public void insertInput(string character)
    {
        InputField InputServer = GameObject.Find("ServerInput").GetComponent<InputField>();
        if (character == "delete" && InputServer.text.Length > 0)
        {
            InputServer.text = InputServer.text.Remove(InputServer.text.Length-1);
        }
        else if(character != "delete")
        {
            InputServer.text += character;
        }
    }

    // Clavier pour insérer le numéro de la salle
    public void insertInputRoom(string character)
    {
        InputField RoomInput = GameObject.Find("RoomInput").GetComponent<InputField>();
        if (character == "delete" && RoomInput.text.Length > 0)
        {
            RoomInput.text = RoomInput.text.Remove(RoomInput.text.Length - 1);
        }
        else if (character != "delete")
        {
            RoomInput.text += character;
        }
    }

    // Connexion au serveur
    public void connectToServer()
    {
        networkManagerScript.connectToServer();
    }


    // Création d'une salle
    public void CreateRoom()
    {
        networkManagerScript.CreateRoom();
    }

    // Rejoindre une salle
    public void JoinRoom()
    {
        networkManagerScript.JoinRoom();
    }

    // Quitter une salle
    public void LeaveRoom()
    {
        networkManagerScript.LeaveRoom();
    }

    // Affichage d'un message sur l'interface utilisateur
    public void errorStatus(string message)
    {
        GameObject.Find("ErrorStatusText").GetComponent<Text>().text = message;
    }

    // Update is called once per frame
    void Update()
    {
        // Afficher/Cacher l'interface en appuyant sur le bouton menu
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Menu))
        {
            if (PlayerUI.activeSelf)
            {
                PlayerUI.SetActive(false);
            }
            else
            {
                PlayerUI.SetActive(true);
                HasOpenedInterface();
            }
        }

        // Mouvement de l'interface par rapport à la caméra
        transform.position = Vector3.SmoothDamp(transform.position, transform.position, ref velocity, smoothTime);
    }
}
