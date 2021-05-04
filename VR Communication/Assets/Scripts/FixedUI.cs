using HTC.UnityPlugin.Vive;
using Photon.Pun;
using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Script permettant de gérer l'interface utilisateur notamment :
// Le texte affiché ( statut de connexion, de salle ... ) 
// Inputs pour entrer l'adresse IP, la salle, le username ..
// La liste des utilisateurs ...
// + Apparition d'un clavier en face de l'IU
public class FixedUI : MonoBehaviour
{
    public GameObject PlayerUI;
    public GameObject ActualRoom;
    public GameObject ButtonConnect;
    public GameObject ServerStatus;
    public GameObject NetworkManager;
    public NetworkManager networkManagerScript;
    public GameObject PresentationButtons;
    public InputField ServerInput;
    public InputField RoomInput;
    public Text RoomActionText;
    public Text ButtonConnectText;
    public Text serverStatusText;
    public Text micStatusText;

    public GameObject KeyBoard;
    public GameObject PlayerCamera;
    public float distanceFromCamera;
    public float heightFromCamera;

    private GameObject vivePointers;
    private GameObject[] drumSticks;
    private GameObject RightHand;
    private GameObject LeftHand;

    public GameObject usernameInterface;
    public GameObject mainInterface;

    // Temps avant de pouvoir fermer ou ouvrir l'interface ( en fps )
    private int availableDelay;
    public int openDelay = 30;
    public int switchAvailableDelay;
    public int switchDelay = 30;

    // Start is called before the first frame update
    void Start()
    {

        RoomActionText = GameObject.Find("CreateJoinRoomButton").GetComponent<Button>().GetComponentInChildren<Text>();
        vivePointers = GameObject.FindGameObjectWithTag("VivePointers");
        drumSticks = GameObject.FindGameObjectsWithTag("DrumStick");
        RightHand = GameObject.FindGameObjectWithTag("RightHand");
        LeftHand = GameObject.FindGameObjectWithTag("LeftHand");
        usernameInterface = GameObject.Find("usernameInterface");
        mainInterface = GameObject.Find("mainInterface");
        PresentationButtons = GameObject.FindGameObjectWithTag("PresentationButtons");
        PlayerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        ActualRoom = GameObject.Find("ActualRoomText");
        ButtonConnect = GameObject.Find("Connect2ServerButton");
        ServerStatus = GameObject.Find("DisconnectedText");
        ServerInput = GameObject.Find("ServerInput").GetComponent<InputField>();
        RoomInput = GameObject.Find("RoomInput").GetComponent<InputField>();
        micStatusText = GameObject.Find("MicStatusButtonText").GetComponent<Text>();

        // Désactivation des drumsticks, les pointers sont activés
        foreach (GameObject drumStick in drumSticks)
        {
            drumStick.SetActive(false);
        }
        
        // Désactivation de l'interface de présentation ( s'activera lorsque l'utilisateur sera dans une salle ET qu'il sera maître de salle ) 
        PresentationButtons.SetActive(false);
        
        NetworkManager = GameObject.Find("Network Manager");
        networkManagerScript = NetworkManager.GetComponent<NetworkManager>();
        
        ButtonConnectText = ButtonConnect.GetComponent<Button>().GetComponentInChildren<Text>();
        serverStatusText = ServerStatus.GetComponent<Text>();

        // On rend l'interface principale non visible.
        PlayerUI.SetActive(false);
        KeyBoard.SetActive(false);
        mainInterface.SetActive(false);

        // Pour le développement
        ServerInput.text = "172.30.16.11";
        RoomInput.text = "1";
    }

    // Mise à jour des données affichées sur l'interface du joueur lorsqu'il appuie sur le bouton pour afficher l'interface
    // Les données sont donc mis à jour à juste avant d'afficher l'interface
    public void HasOpenedInterface()
    {
        // Si l'interface affiché est l'interface principale ( donc après avoir fourni un username )
        if (mainInterface.activeSelf)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                ButtonConnectText.text = "Disconnect";
                serverStatusText.color = Color.green;
                serverStatusText.text = "Connected";
                if (PhotonNetwork.InRoom)
                {
                    ActualRoom.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
                    RoomActionText.text = "Leave";
                }
                else
                {
                    ActualRoom.GetComponent<Text>().text = "Lobby";
                    RoomActionText.text = "Join";
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
        networkManagerScript.JoinOrCreateRoom();
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

    // Confirmation du nom d'utilisateur entré
    public void confirmUsername()
    {
        string usernameText = GameObject.Find("InputUsernameText").GetComponent<Text>().text;
        if (usernameText.Length == 0)
        {
            errorStatus("A username is required.");
        }
        else
        {
            GameObject.Find("KeepAliveEnvironement").GetComponent<KeepAliveObject>().username = GameObject.Find("InputUsernameText").GetComponent<Text>().text;
            errorStatus("");
            usernameInterface.SetActive(false);
            mainInterface.SetActive(true);
        }
        
    }

    // Activation/Désactivation du microphone
    public void enableDisableMic()
    {
        bool isDone;
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("isMine");
            NetworkVoiceManager VoiceScript = Player.GetComponent<NetworkVoiceManager>();
            if (micStatusText.text == "ON")
            {
                isDone = VoiceScript.changeTransmitStatus(false);
                if (isDone == true)
                {
                    micStatusText.text = "OFF";
                }
                else
                {
                    errorStatus("Error disabling mic status");
                }
            }
            else
            {
                isDone = VoiceScript.changeTransmitStatus(true);
                if (isDone == true)
                {
                    micStatusText.text = "ON";
                }
                else
                {
                    errorStatus("Error enabling mic status");
                }
            }
        }
        else
        {
            errorStatus("You need to be connected and in a room");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Temps nécessaire pour pouvoir réutiliser un des boutons de la manette
        // Sinon on a un FPS important, le fait d'appuyer légérement et une seule fois
        // sur le bouton peut provoquer l'ouverture et la fermeture 
        // de l'interface de multiple fois

        if (availableDelay > 0)
        {
            availableDelay -= 1;
        }

        // Switcher entre le mode pointeur pour appuyer sur les bouttons et le mode drumstick pour pouvoir écrire
        if (ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Menu) && ViveInput.GetPress(HandRole.RightHand, ControllerButton.Menu) && availableDelay == 0)
        {
            availableDelay = openDelay;
            if (vivePointers.activeSelf == true)
            {
                vivePointers.SetActive(false);
                foreach (GameObject drumStick in drumSticks)
                {
                    //Debug.Log("Setting drumstick to active");
                    drumStick.SetActive(true);
                }
            }
            else
            {
                vivePointers.SetActive(true);
                foreach (GameObject drumStick in drumSticks)
                {
                    drumStick.SetActive(false);
                }
            }
        }

        // Afficher/Cacher l'interface en appuyant sur le bouton menu de la manette droite
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Menu) && !ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Menu))
        {
            if (KeyBoard.activeSelf && availableDelay == 0)
            {
                availableDelay = openDelay;
                PlayerUI.SetActive(false);
                KeyBoard.SetActive(false);
            }
            else if (!KeyBoard.activeSelf && availableDelay == 0)
            {
                // Pour que l'interface soit toujours en fance de la caméra
                transform.eulerAngles = new Vector3(0, PlayerCamera.transform.eulerAngles.y, 0);
                Vector3 resultingPosition = PlayerCamera.transform.position + PlayerCamera.transform.forward * distanceFromCamera;
                transform.position = new Vector3(resultingPosition.x, transform.position.y, resultingPosition.z);
                availableDelay = openDelay;
                PlayerUI.SetActive(true);
                KeyBoard.transform.position = (RightHand.transform.position + LeftHand.transform.position) / 2 + RightHand.transform.forward * 0.5f;
                KeyBoard.SetActive(true);

                HasOpenedInterface();
            }
        }

        
    }
}
