using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Mic driver used : " + Microphone.UnityMicrophoneDevice);
        //Microphone.is
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
