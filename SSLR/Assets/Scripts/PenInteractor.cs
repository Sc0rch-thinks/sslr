/*
* Author: Shannon Goh and Livinia Poo
* Date: 03/02/2025
* Description: Pen Interactor Handle script
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class PenInteractor : MonoBehaviour
{
    private StampDocument stampDocScript;
    
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
