using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardFollow : MonoBehaviour
{
    [SerializeField] GameObject ToFollow;
    [SerializeField] float distancefromcamera;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = ToFollow.transform.position + ToFollow.transform.forward * 2;
        /*transform.eulerAngles = new Vector3(0, ToFollow.transform.eulerAngles.y, ToFollow.transform.eulerAngles.z);

        Vector3 resultingPosition = ToFollow.transform.position + ToFollow.transform.forward * distancefromcamera;
        transform.position = new Vector3(resultingPosition.x, transform.position.y, resultingPosition.z);
        transform.position = Vector3.SmoothDamp(transform.position, transform.position, ref velocity, smoothTime);*/

    }
}
