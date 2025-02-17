/*
 * Author: Yeo Sai Puay
 * Date: 3/2/25
 * Description: 
 * Stamp Socket Interactor Handle script
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StampSocketInteractor : MonoBehaviour
{
    /// <summary>
    /// Stamp set up
    /// </summary>
    XRSocketInteractor stampSocket;
    public static bool isHeld = false;

    /// <summary>
    /// Assigning stamp socket
    /// </summary>
    private void Awake()
    {
        stampSocket = GetComponent<XRSocketInteractor>();
    }

    /// <summary>
    /// Assigning listeners
    /// </summary>
    void OnEnable()
    {
        stampSocket.selectEntered.AddListener(OnStampTable);
        stampSocket.selectExited.AddListener(OnStampHeld);
    }

    /// <summary>
    /// Removing listeners
    /// </summary>
    private void OnDisable()
    {
        stampSocket.selectEntered.RemoveListener(OnStampTable);
        stampSocket.selectExited.RemoveListener(OnStampHeld);
    }

    /// <summary>
    /// Declare stamp is being held by player
    /// </summary>
    /// <param name="args"></param>
    private void OnStampHeld(SelectExitEventArgs args)
    {
        isHeld = true;
    }

    /// <summary>
    /// Declare stamp is back in socket
    /// </summary>
    /// <param name="args"></param>
    private void OnStampTable(SelectEnterEventArgs args)
    {
        isHeld = false;
    }
    
}
