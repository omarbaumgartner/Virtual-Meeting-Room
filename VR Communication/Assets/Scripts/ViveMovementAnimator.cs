using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class VRMAP
{
    public Transform VRTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        rigTarget.position = VRTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = VRTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class ViveMovementAnimator : MonoBehaviour
{
    [SerializeField]
    public float turnSmoothness;
    public VRMAP head;
    public VRMAP leftHand;
    public VRMAP rightHand;
    public Transform headConstraint;
    public Vector3 headBodyOffset; // Difference of position between head and body
    private PhotonView photonView;
    private Scene m_Scene;

    // Start is called before the first frame update
    void Start()
    {
        m_Scene = SceneManager.GetActiveScene();
        photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine || m_Scene.name == "LobbyScene")
        if (photonView.IsMine)
        { 
            headBodyOffset = transform.position - headConstraint.position;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (photonView.IsMine || m_Scene.name == "LobbyScene")
        if (photonView.IsMine)
        {
            // Move body according to head
            transform.position = headConstraint.position + headBodyOffset;
            transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);
            head.Map();
            leftHand.Map();
            rightHand.Map();
        }    
    }
}
