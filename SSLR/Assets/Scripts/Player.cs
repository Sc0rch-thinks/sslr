/*
 * Author: Livinia Poo
 * Date: 1/2/25
 * Description: 
 * Player Handling
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int score = 0;
    public static int customersServed = 0;
    public static int daysPlayed = 0;

    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void CheckDayEnd()
    {
        if (score < 0)
        {
            gm.dayEnded = true;
            daysPlayed += 1;
            Debug.Log("Game Over, Day has ended");
            Debug.Log(daysPlayed);
        }
    }
}
