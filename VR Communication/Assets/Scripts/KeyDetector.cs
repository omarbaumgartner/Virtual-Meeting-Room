using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshPro playerTextOutput;
    void Start()
    {

            
    }

    private void Update()
    {
        
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wiw");
        // Cherche l'input qui a le tag playerTextOutput ( permet d'avoir plusieurs outputs )
        playerTextOutput = GameObject.FindGameObjectWithTag("playerTextOutput").GetComponent<TextMeshPro>();


        var key = other.GetComponentInChildren<TextMeshPro>();
        if (key != null)
        {

        
        if (other.gameObject.GetComponent<KeyScript>().keyCanBeHitAgain)
        {
            
            if(key.text == "SPACE")
            {
                playerTextOutput.text += " ";
            }
            else if(key.text == "DEL")
            {
                playerTextOutput.text = playerTextOutput.text.Substring(0, playerTextOutput.text.Length - 1);
            }
            else
            {
                playerTextOutput.text += key.text;
            }
        }
        }
    }
}
