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
    [SerializeField]
    private float shiftDuration;
    private float remainingTime;

    private Collider shiftTrigger;
    private GameManager gm;

    void Awake()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
        gm.shiftStarted = false;
        remainingTime = shiftDuration;
        
        shiftTrigger = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !gm.shiftStarted)
        {
            Debug.Log("Shift started");
            
            gm.shiftStarted = true;
            shiftTrigger.enabled = false;
            StartCoroutine(StartWorkShift());
        }
    }

    IEnumerator StartWorkShift()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            /*Debug.Log("Shift: " + remainingTime);*/

            if (Player.score < 0)
            {
                Debug.Log("Too many mistakes! Shift ended!");
                break;
            }
            yield return null;
        }
        
        EndShift();
    }

    void EndShift()
    {
        Debug.Log("Shift ended!");
        
        remainingTime = shiftDuration;
        gm.shiftStarted = false;
    }
}
