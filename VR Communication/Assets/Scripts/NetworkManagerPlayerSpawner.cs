using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkManagerPlayerSpawner : MonoBehaviourPunCallbacks
{

    // Création du joueur lorsqu'il se connecte à une room
    private GameObject spawnedPlayerPrefab;

    // Joining the room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Scene m_Scene = SceneManager.GetActiveScene();
        Debug.Log("Spawning Player prefab in scene : " + m_Scene.name);
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
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
