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
    
    [SerializeField] private GameObject npcObject;

    private string correctDirection;
    private string directionSent;

    void Start()
    {
        npcMoveScript = npcObject.GetComponent<NPCMovement>();
        npcBehaviourScript = npcObject.GetComponent<NPCBehaviour>();
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
            Debug.Log("correct");
        }
        else
        {
            Debug.Log("incorrect");
        }
    }
}
