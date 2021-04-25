using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkPlayerViewer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    private PhotonView photonView;
    [SerializeField] GameObject nickNameTextMP;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        // Attribution du username
        nickNameTextMP.GetComponent<TextMeshPro>().text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (photonView.IsMine)
        {
            GameObject camera = GameObject.Find("Camera");
            GameObject LeftHand = GameObject.Find("LeftHand");
            GameObject RightHand = GameObject.Find("RightHand");            
            MapPosition(head, camera.transform );
            MapPosition(leftHand, LeftHand.transform);
            MapPosition(rightHand, RightHand.transform);
        }
    }

    // Remplace la position des transform.view en la position des manettes
    void MapPosition(Transform target, Transform device)
    {
        target.position = device.position;
    }
}
