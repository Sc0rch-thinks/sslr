using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampInteractor : MonoBehaviour
{
    public string stampName;

    void Awake()
    {
        stampName = gameObject.name;
    }
    
   private void OnCollisionEnter(Collision collision)
   {
       if (StampSocketInteractor.isHeld && collision.gameObject.CompareTag("Paper"))
       {
           Debug.Log("paper is stamped");

           if (stampName == "Stamp_Financial")
           {
               Debug.Log("Financial Document");
           }
           else if (stampName == "Stamp_Residential")
           {
               Debug.Log("Residential Document");
           }
           else if (stampName == "Stamp_DV")
           {
               Debug.Log("Domestic Violence Document");
           }
           else if (stampName == "Stamp_Disabilities")
           {
               Debug.Log("Disabilities Document");
           }
       }
   }
}
