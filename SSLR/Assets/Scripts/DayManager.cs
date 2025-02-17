/*
 * Author: Livinia Poo
 * Date: 4/2/25
 * Description:
 * Managing start and end days
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public bool doneAShift = false;

    [SerializeField] private GameObject shiftManager;

    private Collider endDayTrigger;
    private GameManager gm;
    private ShiftManager shiftManagerScript;

    void Awake()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        shiftManagerScript = shiftManager.GetComponent<ShiftManager>();
        endDayTrigger = GetComponent<Collider>();

        endDayTrigger.enabled = false;
    }

    void Update()
    {
        if (doneAShift && !endDayTrigger.enabled)
        {
            endDayTrigger.enabled = true;
        }
        else if (!doneAShift && endDayTrigger.enabled)
        {
            endDayTrigger.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Day completed!");
            Player.daysPlayed += 1;
            Debug.Log(Player.daysPlayed);
            shiftManagerScript.AllowShiftStart();
            Debug.Log("You can start another shift!");

            doneAShift = false;
        }
    }
}