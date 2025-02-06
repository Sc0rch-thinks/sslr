using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampInteractor : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
       if (StampSocketInteractor.isHeld && collision.gameObject.CompareTag("Paper"))
       {
           Debug.Log("paper is stamped");
       }
       /*else
       {
           Debug.LogError("something aint right");
       }*/
   }
}
