/*
 * Author: Livinia Poo
 * Date: 29/1/25
 * Description: 
 * Managing behaviour of VR buttons at desk
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskButtons : MonoBehaviour
{
    NPCMovement npcMoveScript;
    NPCBehaviour npcBehaviourScript;
    private Player playerScript;
    
    [SerializeField] private GameObject npcObject;

    private string correctDirection;
    private string directionSent;

    void Start()
    {
        npcMoveScript = npcObject.GetComponent<NPCMovement>();
        npcBehaviourScript = npcObject.GetComponent<NPCBehaviour>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void DirectToLeft()
    {
        npcMoveScript.WalkToPlayerLeft();
        directionSent = "left";

        if (npcBehaviourScript.navigationTxt.text == "I want to go left!")
        {
            correctDirection = "left";
        }
        else
        {
            correctDirection = "right";
        }

        CheckCorrectDirection();
    }

    public void DirectToRight()
    {
        npcMoveScript.WalkToPlayerRight();
        directionSent = "right";
        
        if (npcBehaviourScript.navigationTxt.text == "I want to go right!")
        {
            correctDirection = "right";
        }
        else
        {
            correctDirection = "left";
        }
        
        CheckCorrectDirection();
        
    }

    void CheckCorrectDirection()
    {
        if (correctDirection == directionSent)
        {
            Player.score += 1;
            Player.customersServed += 1;
            Debug.Log("correct");
            Debug.Log(Player.score);
            Debug.Log(Player.customersServed);
        }
        else
        {
            Player.score -= 1;
            Player.customersServed += 1;
            Debug.Log("incorrect");
            Debug.Log(Player.score);
            Debug.Log(Player.customersServed);
            
            playerScript.CheckDayEnd();
        }
    }
}
