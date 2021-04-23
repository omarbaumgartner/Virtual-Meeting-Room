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
        yOriginalPosition = transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyHit)
        {
            keyCanBeHitAgain = false;
            keyHit = false;
            transform.position += new Vector3(0, -0.03f, 0);
        }

        // La touche remonte
        if(transform.position.y < yOriginalPosition)
        {
            transform.position += new Vector3(0, 0.001f, 0);
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
