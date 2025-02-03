/*
* Author: Shannon Goh 
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
    private void OnCollisionEnter(Collision collision)
    {
        if (PenSocketInteractor.isPickedUp && collision.gameObject.CompareTag("Paper"))
        {
            Debug.Log("The pen is writing on the paper!");
        }
    }

}
