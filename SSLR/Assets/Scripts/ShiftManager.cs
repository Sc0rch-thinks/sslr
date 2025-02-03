/*
 * Author: Livinia Poo
 * Date: 4/2/25
 * Description: 
 * Starting/Ending Shifts
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    private bool shiftStarted;
    [SerializeField]
    private float shiftDuration;
    private float remainingTime;

    private Collider shiftTrigger;

    void Awake()
    {
        shiftStarted = false;
        remainingTime = shiftDuration;
        
        shiftTrigger = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !shiftStarted)
        {
            Debug.Log("Shift started");
            
            shiftStarted = true;
            shiftTrigger.enabled = false;
            StartCoroutine(StartWorkShift());
        }
    }

    IEnumerator StartWorkShift()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            
            Debug.Log("Shift: " + remainingTime);
            yield return null;
        }
        
        EndShift();
    }

    void EndShift()
    {
        Debug.Log("Shift ended!");
        
        /*
        shiftTrigger.enabled = true;
        */
        remainingTime = shiftDuration;
        shiftStarted = false;
    }
}
