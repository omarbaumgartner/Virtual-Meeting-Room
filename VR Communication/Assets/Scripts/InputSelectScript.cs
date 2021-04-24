using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
