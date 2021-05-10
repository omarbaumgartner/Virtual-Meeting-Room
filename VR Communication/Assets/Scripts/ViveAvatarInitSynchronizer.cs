using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViveAvatarInitSynchronizer : MonoBehaviour
{
    private ViveMovementAnimator VRigScript;
    private PhotonView photonView;

    // Synchronisation des manettes + casque avec l'avatar lors de la création de ce dernier
    
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
            ViveMovementAnimator animation = gameObject.GetComponent<ViveMovementAnimator>();
            animation.headBodyOffset.x = 0;
            animation.headBodyOffset.y = -1.65f;
            animation.headBodyOffset.z = 0;
            Debug.Log(animation.headBodyOffset);
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
