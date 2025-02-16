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
    [SerializeField]
    private GameObject npcSpawnArea;

    private Collider shiftTrigger;
    private GameManager gm;
    private DayManager dayManager;

    void Awake()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        dayManager = GameObject.Find("Day Manager").GetComponent<DayManager>();
        
        gm.shiftStarted = false;
        remainingTime = shiftDuration;
        
        shiftTrigger = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gm.shiftStarted)
        {
            Debug.Log("Shift started");
            
            npcSpawnArea.SetActive(true);
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
        
        npcSpawnArea.SetActive(false);
        remainingTime = shiftDuration;
        gm.shiftStarted = false;
        dayManager.doneAShift = true;
        NpcManager.instance.EndDay();
    }

    public void AllowShiftStart()
    {
        shiftTrigger.enabled = true;
    }
}
