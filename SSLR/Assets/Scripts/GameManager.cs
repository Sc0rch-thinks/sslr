/*
 * Author: Livinia Poo
 * Date: 24/1/25
 * Description: 
 * Game Manager
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Assign GM instance
    /// </summary>
    public static GameManager instance;
    
    /// <summary>
    /// References and Variables
    /// </summary>
    public bool dayEnded = false;
    public bool shiftStarted;

    /// <summary>
    /// NPC in front of desk
    /// </summary>
    public GameObject currentNPC;

    public string currentNPCCorrectDepartment;

    ///<summary>
    /// References for player-npc dialogue
    /// </summary>    
    [Header("NPC Dialogue")]
    public TextMeshProUGUI playerQuestionOneText;
    public TextMeshProUGUI playerQuestionTwoText;
    public TextMeshProUGUI playerQuestionThreeText;

    public TextMeshProUGUI playerResponse;

    public GameObject playerDialogue;

    /// <summary>
    /// Do Not Destroy on Load
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        playerDialogue.SetActive(false);
    }

    /// <summary>
    /// Setting NPC in front of desk as current
    /// </summary>
    /// <param name="npc"></param>
    public void SetCurrentNPC(GameObject npc)
    {
        currentNPC = npc;
        
        NpcMovementRework npcScript = npc.GetComponent<NpcMovementRework>();
        if (npcScript != null)
        {
            currentNPCCorrectDepartment = npcScript.correctService;
            Debug.Log($"Current NPC service: {currentNPCCorrectDepartment}");
        }
        else
        {
            Debug.LogError("NPC Script not found on NPC");
        }
    }
}
