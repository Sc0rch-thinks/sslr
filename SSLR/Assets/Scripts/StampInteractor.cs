using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampInteractor : MonoBehaviour
{
    public string stampName;

    [SerializeField] private GameObject stampDoc;
    private StampDocument stampDocScript;

    void Awake()
    {
        stampName = gameObject.name;
        
        stampDocScript = stampDoc.GetComponent<StampDocument>();
    }
    
   private void OnCollisionEnter(Collision collision)
   {
       if (StampSocketInteractor.isHeld && collision.gameObject.CompareTag("Paper"))
       {
           Debug.Log("paper is stamped");

           if (stampName == "Stamp_Financial")
           {
               stampDocScript.StampFinancial();
           }
           else if (stampName == "Stamp_Residential")
           {
               stampDocScript.StampResidential();
           }
           else if (stampName == "Stamp_DV")
           {
               stampDocScript.StampDV();
           }
           else if (stampName == "Stamp_Disabilities")
           {
               stampDocScript.StampDisabilities();
           }
       }
   }
}
