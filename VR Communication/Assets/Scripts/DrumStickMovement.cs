using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script permettant de synchroniser la position et la rotation des DrumStick avec les manettes.
public class DrumStickMovement : MonoBehaviour
{
    [SerializeField] GameObject ToFollow;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ToFollow.transform.position;
        transform.rotation = ToFollow.transform.rotation * Quaternion.Euler(0, 90f, 90f);
    }
}
