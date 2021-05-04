using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script qui prend en charge l'insertion des caractères des touches tapés sur le clavier dans l'input sélectionné
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
                        playerTextOutput.text += " ";
                    }
                    else if (key.text == "DEL")
                    {
                        if (playerTextOutput.text.Length > 0)
                        {
                            playerTextOutput.text = playerTextOutput.text.Substring(0, playerTextOutput.text.Length - 1);
                        }
                    }
                    else
                    {
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
