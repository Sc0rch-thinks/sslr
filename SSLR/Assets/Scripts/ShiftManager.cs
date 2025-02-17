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
    /// <summary>
    /// Shift details
    /// </summary>
    [SerializeField] private float shiftDuration;
    private float remainingTime;
    [SerializeField] private GameObject npcSpawnArea;

    /// <summary>
    /// Shift box collider trigger
    /// </summary>
    private Collider shiftTrigger;
    
    /// <summary>
    /// Gamemanager reference 
    /// </summary>
    private GameManager gm;
    
    /// <summary>
    /// Day manager reference
    /// </summary>
    private DayManager dayManager;

    /// <summary>
    /// Assigning variables and references
    /// </summary>
    void Awake()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        dayManager = GameObject.Find("Day Manager").GetComponent<DayManager>();

        gm.shiftStarted = false;
        remainingTime = shiftDuration;

        shiftTrigger = GetComponent<Collider>();
    }

    /// <summary>
    /// Start shift on trigger
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// Timer to count down shift end
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Stop all shift-related logic
    /// </summary>
    void EndShift()
    {
        Debug.Log("Shift ended!");

        npcSpawnArea.SetActive(false);
        remainingTime = shiftDuration;
        gm.shiftStarted = false;
        dayManager.doneAShift = true;
        NpcManager.instance.EndDay();
    }

    /// <summary>
    /// Start next day shift
    /// </summary>
    public void AllowShiftStart()
    {
        shiftTrigger.enabled = true;
    }
}