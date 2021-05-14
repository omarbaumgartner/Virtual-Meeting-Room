using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyNameSpace
{

    public class NetworkChairManager : MonoBehaviour
    {
        bool playerIsSitting = false;
        int takenChairIndex;
        GameObject[] chairs;
        string[] chairsNames;
        bool[] chairsStates;
        GameObject Player;
        GameObject PlayerCamera;
        Vector3 offset = new Vector3(0, 0.5f, 0);
        PhotonView photonView;


        // Start is called before the first frame update
        void Start()
        {


            photonView = GetComponent<PhotonView>();
            chairs = GameObject.FindGameObjectsWithTag("Chair");
            chairsNames = new string[chairs.Length];
            chairsStates = new bool[chairs.Length];
            initChairs();


            Player = GameObject.Find("VROrigin");
            PlayerCamera = GameObject.Find("Camera");
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void sitInChair()
        {
            foreach (GameObject chair in chairs)
            {
                // Check free chair
                int index = 0;
                if (chair.GetComponent<chairState>().isTaken == false)
                {
                    // Teleport player to chair position
                    teleportTo(chair);

                    // Freeze player TODO
                    // UnityEngine.XR.InputTracking.disablePositionalTracking = true;

                    // Set chair to taken
                    playerIsSitting = true;
                    takenChairIndex = index;
                    photonView.RPC("setChairTo", RpcTarget.All, chair.transform.name, true);
                    int i = System.Array.IndexOf(chairsNames, chair.transform.name);
                    chairsStates[i] = true;
                    //chair.GetComponent<chairState>().isTaken = true;
                    return;
                }
                index += 1;
            }
            Debug.Log("There's no available chair");
            return;
        }

        public void freeChair()
        {
            if (playerIsSitting)
            {
                Debug.Log("Player was sitting, freeing chair");
                photonView.RPC("setChairTo", RpcTarget.All, chairs[takenChairIndex].transform.name, false);
                int i = System.Array.IndexOf(chairsNames, chairs[takenChairIndex].transform.name);
                chairsStates[i] = false;
                playerIsSitting = false;
            }
            else
            {
                Debug.Log("Player is not sitting, doing nothing");
            }
        }

        // Initialise la liste des noms et des états à chaque création de room
        public void initChairs()
        {
            for (int i = 0; i < chairs.Length; i++)
            {
                chairsNames[i] = chairs[i].transform.name;
                chairsStates[i] = chairs[i].GetComponent<chairState>().isTaken;
            }
        }

        // Change l'état d'une chaise lorsqu'elle vient d'être prise/libérée
        [PunRPC]
        void setChairTo(string chairName, bool isTaken)
        {
            GameObject.Find(chairName).GetComponent<chairState>().isTaken = isTaken;
        }

        public void sendChairsInfos()
        {
            photonView.RPC("updateChairsState", RpcTarget.All, chairsNames, chairsStates);

        }

        // Transmet l'état des chaises déjà prises/libérés depuis le Master vers les nouveaux utilisateurs qui ont rejoint la salle
        [PunRPC]
        void updateChairsState(string[] chairsName, bool[] chairsStates)
        {
            for (int i = 0; i < chairsName.Length; i++)
            {
                GameObject.Find(chairsName[i]).GetComponent<chairState>().isTaken = chairsStates[i];
            }
        }

        public void masterUpdateChairs()
        {


        }

        void teleportTo(GameObject target)
        {
            Vector3 GlobalCameraPosition = PlayerCamera.transform.position;  //get the global position of VRcamera
            Vector3 GlobalPlayerPosition = Player.transform.position;
            Vector3 GlobalOffsetCameraPlayer = new Vector3(GlobalCameraPosition.x - GlobalPlayerPosition.x, GlobalCameraPosition.y - GlobalPlayerPosition.y, GlobalCameraPosition.z - GlobalPlayerPosition.z) + offset;
            Vector3 newRigPosition = new Vector3(target.transform.position.x - GlobalOffsetCameraPlayer.x, (target.transform.position.y + 1.5f) - GlobalOffsetCameraPlayer.y, target.transform.position.z - GlobalOffsetCameraPlayer.z);
            Player.transform.position = newRigPosition;
        }
    }
}
