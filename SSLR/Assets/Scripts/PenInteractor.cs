/*
* Author: Shannon Goh and Livinia Poo
* Date: 03/02/2025
* Description:
* Pen Interactor Handle script
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class PenInteractor : MonoBehaviour
{
    /// <summary>
    /// Document script reference
    /// </summary>
    private StampDocument stampDocScript;
    
    /// <summary>
    /// Sign when pen touches area
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (PenSocketInteractor.isPickedUp && collision.gameObject.CompareTag("Paper"))
        {
            if (collision.collider.gameObject.name == "Stamp-Sign Area")
            {
                stampDocScript = collision.gameObject.GetComponent<StampDocument>();
                
                if(stampDocScript != null)
                {
                    if(!stampDocScript.isSigned)
                    {
                        PlayerDialogueInteraction.instance.servicesPanel.SetActive(true);
                    }                
                }
                else
                {
                    Debug.LogError("Paper doesn't have StampDocScript");
                }
            }
        }
    }

}
