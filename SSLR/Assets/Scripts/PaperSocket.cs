/*
 * Author: Livinia Poo
 * Date: 1/2/25
 * Description: 
 * Paper  Handling
 Interaction*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PaperSocket : MonoBehaviour
{
    [SerializeField] private string socketName;
    XRSocketInteractor interactor;

    void Awake()
    {
        interactor = GetComponent<XRSocketInteractor>();
    }
    
    void OnEnable()
    {
        interactor.selectEntered.AddListener(OnPaperAttached);
        interactor.selectExited.AddListener(OnPaperDetached);
    }
    
    void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnPaperAttached);
        interactor.selectExited.RemoveListener(OnPaperDetached);
    }

    private void OnPaperAttached(SelectEnterEventArgs args)
    {
        if (socketName == "Paper Socket_NPC")
        {
            Debug.Log("Paper returned to NPC");
        }
        else if (socketName == "Paper Socket")
        {
            Debug.Log("Paper placed on table");
        }
    }
    
    private void OnPaperDetached(SelectExitEventArgs args)
    {
        if (socketName == "Paper Socket_NPC")
        {
            Debug.Log("Paper taken from NPC");
        }
        else if (socketName == "Paper Socket")
        {
            Debug.Log("Paper removed from table");
        }
    }
}
