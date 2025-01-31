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

    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    void Update()
    {
        CheckDayEnd();
    }

    void CheckDayEnd()
    {
        if (score <= 0)
        {
            Debug.Log("Game Over, Day has ended");
            gm.dayEnded = true;
        }
    }
}
