using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampInteractor : MonoBehaviour
{
    public string stampName;

    private StampDocument stampDocScript;

    void Awake()
    {
        stampName = gameObject.name;
    }
    
   private void OnCollisionEnter(Collision collision)
   {
       if (StampSocketInteractor.isHeld && collision.gameObject.CompareTag("Paper"))
       {
           if (collision.collider.gameObject.name == "Stamp-Sign Area")
           {
               stampDocScript = collision.gameObject.GetComponent<StampDocument>();
               
               if(stampDocScript != null)
               {
                   if (stampName == "Stamp_Financial" && !stampDocScript.isStamped)
                   {
                       stampDocScript.StampFinancial();
                   }
                   else if (stampName == "Stamp_Residential" && !stampDocScript.isStamped)
                   {
                       stampDocScript.StampResidential();
                   }
                   else if (stampName == "Stamp_DV" && !stampDocScript.isStamped)
                   {
                       stampDocScript.StampDV();
                   }
                   else if (stampName == "Stamp_Disabilities" && !stampDocScript.isStamped)
                   {
                       stampDocScript.StampDisabilities();
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
