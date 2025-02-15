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
        GameObject paperObject = args.interactableObject.transform.gameObject;
        string paperName = paperObject.name;
        
        if (socketName == "Paper Socket_NPC")
        {
            if (paperName == "Spawnable_Doc")
            {
                Debug.Log("Document given");
                Destroy(paperObject);
            }

            if (paperName == "NPC_Doc")
            {
                return;
            }
        }
        else if (socketName == "Paper Socket")
        {
            if (paperName == "Spawnable_Doc")
            {
                return;
            }

            if (paperName == "NPC_Doc")
            {
                Debug.Log("Document submitted");
                Destroy(paperObject);
            }
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
