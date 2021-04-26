using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private InputField playerTextOutput;
    void Start()
    {


    }

    private void Update()
    {

    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        //playerTextOutput = GameObject.FindGameObjectWithTag("playerTextOutput").GetComponent<Text>();

        var key = other.GetComponentInChildren<TextMeshPro>();
        if (key != null)
        {
            if (other.gameObject.GetComponent<KeyScript>().keyCanBeHitAgain)
            {
                // Cherche l'input qui a le tag playerTextOutput ( permet d'avoir plusieurs outputs )

                GameObject TextOuput = GameObject.FindGameObjectWithTag("playerTextOutput");

                if (TextOuput != null)
                {
                    playerTextOutput = TextOuput.GetComponent<InputField>();
                    if (key.text == "SPACE")
                    {
                        //Debug.Log("Adding space");
                        playerTextOutput.text += " ";
                    }
                    else if (key.text == "DEL")
                    {
                        if (playerTextOutput.text.Length > 0)
                        {
                            //Debug.Log("Deleting");
                            playerTextOutput.text = playerTextOutput.text.Substring(0, playerTextOutput.text.Length - 1);
                        }
                    }
                    else
                    {
                        //Debug.Log("Adding "+key.text);
                        playerTextOutput.text += key.text;
                    }
                }
                else
                {
                    //Debug.Log("playerTextOutput is null");
                }
            }
        }
    }
}
