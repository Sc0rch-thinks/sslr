/*
 * Author: Livinia Poo
 * Date: 1/2/25
 * Description: 
 * Paper Socket Logic
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PaperSocket : MonoBehaviour
{
    /// <summary>
    /// Variables
    /// </summary>
    [SerializeField] private string socketName;
    XRSocketInteractor interactor;

    /// <summary>
    /// Assigning variables
    /// </summary>
    void Awake()
    {
        interactor = GetComponent<XRSocketInteractor>();
    }
    
    /// <summary>
    /// Adding listeners
    /// </summary>
    void OnEnable()
    {
        interactor.selectEntered.AddListener(OnPaperAttached);
        interactor.selectExited.AddListener(OnPaperDetached);
    }
    
    /// <summary>
    /// Removing listeners
    /// </summary>
    void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnPaperAttached);
        interactor.selectExited.RemoveListener(OnPaperDetached);
    }

    /// <summary>
    /// Attached logic
    /// </summary>
    /// <param name="args"></param>
    private void OnPaperAttached(SelectEnterEventArgs args)
    {
        GameObject paperObject = args.interactableObject.transform.gameObject;
        string paperName = paperObject.name;
        
        if (socketName == "Paper Socket_NPC")
        {
            if (paperName == "Spawnable_Doc")
            {
                StampDocument stampDoc = paperObject.GetComponent<StampDocument>();

                if (stampDoc != null)
                {
                    if (stampDoc.isStamped && stampDoc.isSigned)
                    {
                        Debug.Log("Document given");
                        PlayerDialogueInteraction.instance.takeDocumentsTick.SetActive(true);
                        PlayerDialogueInteraction.instance.giveDocumentsTick.SetActive(true);
                        Destroy(paperObject);
                    }
                    else
                    {
                        Debug.Log("Either not stamped or not signed");
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Removed logic
    /// </summary>
    /// <param name="args"></param>
    private void OnPaperDetached(SelectExitEventArgs args)
    {
        if (socketName == "Paper Socket_NPC")
        {
            Debug.Log("Paper taken from NPC");
        }
    }
}
