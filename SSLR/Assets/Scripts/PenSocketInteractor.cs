/*
* Author: Shannon Goh 
* Date: 03/02/2025
* Description: Pen Socket Interactor Handle script
*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PenSocketInteractor : MonoBehaviour
{
    XRSocketInteractor penSocket;
    public static bool isPickedUp = false;

    private void Awake()
    {
        penSocket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        penSocket.selectEntered.AddListener(OnPenPlaced);
        penSocket.selectExited.AddListener(OnPenPickedUp);
    }

    void OnDisable()
    {
        penSocket.selectEntered.RemoveListener(OnPenPlaced);
        penSocket.selectExited.RemoveListener(OnPenPickedUp);
    }
    
    private void OnPenPickedUp(SelectExitEventArgs args)
    {
        isPickedUp = true;
        Debug.Log("Pen picked up");
    }

    private void OnPenPlaced(SelectEnterEventArgs args)
    {
        isPickedUp = false;
        Debug.Log("Pen placed back");
    }
}
