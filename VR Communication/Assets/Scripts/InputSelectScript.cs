using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script permettant de gérer le focus sur un des inputs de l'IU.
// Lorsque l'utilisateur clique avec le pointeur sur un des inputs, ce dernier a son tag qui change en "playerTextOutput"
// et qui pourra être géré par les autres scripts ( afin d'y insérer les caractères )
public class InputSelectScript : MonoBehaviour
{

    private GameObject childText;
    // Start is called before the first frame update
    void Start()
    {
        childText = gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject;

    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            InputField input = gameObject.GetComponent<InputField>();
            if (input.isFocused == true)
            {
                gameObject.tag = "playerTextOutput";
            }
            else
            {
                gameObject.tag = "Untagged";
            }
        }
    }
}
