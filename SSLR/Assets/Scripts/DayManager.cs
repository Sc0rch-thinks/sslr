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
    /// <summary>
    /// Whether the player has completed the shift
    /// </summary>
    public bool doneAShift = false;

    /// <summary>
    /// Shiftmanager reference
    /// </summary>
    [SerializeField] private GameObject shiftManager;
    private ShiftManager shiftManagerScript;
    
    /// <summary>
    /// GameManager reference
    /// </summary>
    private GameManager gm;

    /// <summary>
    /// Box collider trigger
    /// </summary>
    private Collider endDayTrigger;

    /// <summary>
    /// Assigning references
    /// </summary>
    void Awake()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        shiftManagerScript = shiftManager.GetComponent<ShiftManager>();
        endDayTrigger = GetComponent<Collider>();

        endDayTrigger.enabled = false;
    }

    /// <summary>
    /// Check if end day collider activates
    /// </summary>
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

    /// <summary>
    /// End the day
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Day completed!");
            Backend.instance.User.daysPlayed += 1;
            Debug.Log(Backend.instance.User.daysPlayed);
            shiftManagerScript.AllowShiftStart();
            Debug.Log("You can start another shift!");

            doneAShift = false;
        }
    }
}