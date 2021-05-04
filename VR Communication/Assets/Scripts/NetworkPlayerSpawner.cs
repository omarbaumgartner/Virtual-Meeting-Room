using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// Script gérant les apparitions des joueurs lorsqu'ils rejoignent une salle
public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    // Création du joueur lorsqu'il se connecte à une room
    private GameObject spawnedPlayerPrefab;

    
    // Se déclenche lorsqu'un joueur entre dans une salle
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // Instantiantion de l'avatar du joueur
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
        PhotonView view = spawnedPlayerPrefab.GetComponent<PhotonView>();
        // Attribution du tag isMine si le joueur entrant dans la salle est l'utilisateur de l'application
        if (view.IsMine)
        {
            spawnedPlayerPrefab.tag = "isMine";
            // Pour que la texture de l'avatar ne dérange pas la vision du joueur
            // Il ne verra pas son avatar mais les autres le verront
            spawnedPlayerPrefab.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            
        }
    }

    // Leaving the room
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        // Destroy the prefab for all of the cliets of the server
        if(spawnedPlayerPrefab != null)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
        }
    }

}
