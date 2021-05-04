using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script permettant d'activer ou de désactiver la transmission du son venant du microphone vers les autres utilisateurs
// ( Est utilisé au niveau de l'IU lorsque l'on appuie sur le bouton ON/OFF ) 
public class NetworkVoiceManager : MonoBehaviour
{
    Recorder Microphone;
    // Start is called before the first frame update
    void Start()
    {
        Microphone = this.GetComponent<Recorder>();
        Microphone.TransmitEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool changeTransmitStatus(bool status)
    {
        try
        {
            Microphone.TransmitEnabled = status;
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

}
