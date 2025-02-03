using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampInteractor : MonoBehaviour
{
   private void OnCollisionEnter(Collision other)
   {
       if (StampSocketInteractor.isHeld && other.gameObject.CompareTag("Paper"))
       {
           Debug.Log("paper is stamped");
       }
   }
}
