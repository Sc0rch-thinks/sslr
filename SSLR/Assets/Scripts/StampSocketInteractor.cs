using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StampSocketInteractor : MonoBehaviour
{
    XRSocketInteractor stampSocket;
    public static bool isHeld = false;


    private void Awake()
    {
        stampSocket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        stampSocket.selectEntered.AddListener(OnStampTable);
        stampSocket.selectExited.AddListener(OnStampHeld);
    }

    private void OnDisable()
    {
        stampSocket.selectEntered.RemoveListener(OnStampTable);
        stampSocket.selectExited.RemoveListener(OnStampHeld);
    }


    private void OnStampHeld(SelectExitEventArgs args)
    {
        isHeld = true;
        Debug.Log("Stamp is held");
        /*Debug.Log(isHeld.ToString());*/
    }

    private void OnStampTable(SelectEnterEventArgs args)
    {
        isHeld = false;
        Debug.Log("Stamp is on table");
    }
    
}
