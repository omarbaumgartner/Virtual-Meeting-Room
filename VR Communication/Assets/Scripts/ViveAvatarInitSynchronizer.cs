using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViveAvatarInitSynchronizer : MonoBehaviour
{
    private ViveMovementAnimator VRigScript;
    private PhotonView photonView;

    // Synchronisation des manettes + casque avec l'avatar lors de la création de ce dernier
    // Start is called before the first frame update
    void Start()
    {
        Scene m_Scene = SceneManager.GetActiveScene();
        string sceneName = m_Scene.name;
        Debug.Log("Scene name : " + sceneName);
        photonView = GetComponent<PhotonView>();
        VRigScript = GetComponent<ViveMovementAnimator>();
        Debug.Log("photonView.IsMine : " + photonView.IsMine);
        if (photonView.IsMine)
        {
            Debug.Log("Synchronizing robot and rig");
            GameObject camera = GameObject.Find("Camera");
            GameObject LeftHand = GameObject.Find("LeftHand");
            GameObject RightHand = GameObject.Find("RightHand");
            VRigScript.head.VRTarget = camera.transform;
            VRigScript.leftHand.VRTarget = LeftHand.transform;
            VRigScript.rightHand.VRTarget = RightHand.transform;
        }
        else
        {
            VRigScript.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
