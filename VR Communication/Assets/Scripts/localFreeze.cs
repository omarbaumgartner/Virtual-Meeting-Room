using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localFreeze : MonoBehaviour
{
    private float xPosition;
    private float yPosition;
    // Start is called before the first frame update
    void Start()
    {
        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.position = new Vector3(xPosition, yPosition, transform.position.z);
        if (transform.position.z > 21.76f) {
            transform.position = new Vector3(xPosition, yPosition, 21.76f);
        }
        if (transform.position.z < 7.51f)
        {
            transform.position = new Vector3(xPosition, yPosition, 7.51f);
        }
    }
}
