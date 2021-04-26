using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool keyHit = false;
    public bool keyCanBeHitAgain = false;

    private float yOriginalPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf && yOriginalPosition == float.NaN)
        {
            yOriginalPosition = transform.position.y;
        }
        else if(!gameObject.activeSelf)
        {
            yOriginalPosition = float.NaN;
        }

        if (keyHit && keyCanBeHitAgain && yOriginalPosition != float.NaN)
        {
            yOriginalPosition = transform.position.y;
            keyCanBeHitAgain = false;
            keyHit = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
        }

        // La touche remonte
        if (transform.position.y < yOriginalPosition)
        {
            transform.position += new Vector3(0, 0.005f, 0);
        }
        // Quand la touche sera entiérement remontée, on pourra retaper sur la touche
        else
        {
            keyCanBeHitAgain = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        keyHit = true;
    }

}
