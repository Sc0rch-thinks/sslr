/*
 * Author: Livinia Poo
 * Date: 1/2/25
 * Description: 
 * Dialogue Interaction (Player side)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueInteraction : MonoBehaviour
{
    [Header("Checklist Ticks")] 
    public GameObject welcomeTick;
    public GameObject situationTick;
    public GameObject takeDocumentsTick;
    public GameObject servicesTick;
    public GameObject giveDocumentsTick;

    /// <summary>
    /// Resets checklist when npc is finished
    /// </summary>
    public void ResetChecklist()
    {
        welcomeTick.SetActive(false);
        situationTick.SetActive(false);
        takeDocumentsTick.SetActive(false);
        servicesTick.SetActive(false);
        giveDocumentsTick.SetActive(false);
    }

    public void ResetDialogue()
    {
        GameManager.instance.playerDialogue.SetActive(false);
    }
    
    
}
