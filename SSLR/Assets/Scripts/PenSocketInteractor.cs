/*
* Author: Shannon Goh 
* Date: 03/02/2025
* Description:
* Pen Socket Interactor Handle script
*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PenSocketInteractor : MonoBehaviour
{
    /// <summary>
    /// Pen set up
    /// </summary>
    XRSocketInteractor penSocket;
    public static bool isPickedUp = false;
    
    /// <summary>
    /// Assigning pen socket
    /// </summary>
    private void Awake()
    {
        penSocket = GetComponent<XRSocketInteractor>();
    }

    /// <summary>
    /// Adding listeners
    /// </summary>
    void OnEnable()
    {
        penSocket.selectEntered.AddListener(OnPenPlaced);
        penSocket.selectExited.AddListener(OnPenPickedUp);
    }

    /// <summary>
    /// Removing listeners
    /// </summary>
    void OnDisable()
    {
        penSocket.selectEntered.RemoveListener(OnPenPlaced);
        penSocket.selectExited.RemoveListener(OnPenPickedUp);
    }
    
    /// <summary>
    /// Declare that pen is picked up
    /// </summary>
    /// <param name="args"></param>
    private void OnPenPickedUp(SelectExitEventArgs args)
    {
        isPickedUp = true;
    }

    /// <summary>
    /// Declare pen is returned back
    /// </summary>
    /// <param name="args"></param>
    private void OnPenPlaced(SelectEnterEventArgs args)
    {
        isPickedUp = false;
    }
}
