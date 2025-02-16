/*
 * Author: Livinia Poo
 * Date: 1/2/25
 * Description: 
 * Dialogue Interaction (Player side)
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDialogueInteraction : MonoBehaviour
{
    public static PlayerDialogueInteraction instance;

    ///<summary>
    /// References for player-npc dialogue
    /// </summary>    
    [Header("Player UI")] 
    public GameObject playerDialogue;
    public GameObject welcomeButton;
    public GameObject questionPanel;
    public GameObject responsePanel;
    public GameObject servicesPanel;
    
    public TextMeshProUGUI playerQuestionOneText;
    public TextMeshProUGUI playerQuestionTwoText;
    public TextMeshProUGUI playerQuestionThreeText;
    public TextMeshProUGUI playerResponseText;

    private bool question1Asked = false;
    private bool question2Asked = false;
    private bool question3Asked = false;

    [Header("Checklist UI")] 
    public GameObject welcomeTick;
    public GameObject situationTick;
    public GameObject serviceTick;
    public GameObject takeDocumentsTick;
    public GameObject giveDocumentsTick;

    [Header("Services")] 
    public GameObject comcareServiceButton;
    public GameObject fscServiceButton;
    public GameObject peersServiceButton;
    public GameObject transitionalSheltersServiceButton;
    public GameObject cpsServiceButton;
    public GameObject childrenYoungHomeServiceButton;
    public GameObject sgEnableServiceButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ResetDialogue();
        ResetChecklist();
    }

    public void ResetDialogue()
    {
        playerDialogue.SetActive(false);
        questionPanel.SetActive(false);
        responsePanel.SetActive(false);
        
        welcomeButton.SetActive(true);
        
        question1Asked = false;
        question2Asked = false;
        question3Asked = false;
    }

    public void ResetChecklist()
    {
        welcomeTick.SetActive(false);
        situationTick.SetActive(false);
        serviceTick.SetActive(false);
        giveDocumentsTick.SetActive(false);
        takeDocumentsTick.SetActive(false);
    }

    public void SetPlayerQuestions(string q1, string q2, string q3)
    {
        playerQuestionOneText.text = q1;
        playerQuestionTwoText.text = q2;
        playerQuestionThreeText.text = q3;
    }

    public void SetPlayerResposne(string response)
    {
        playerResponseText.text = response;
    }

    public void WelcomePressed()
    {
        questionPanel.SetActive(true);
        welcomeButton.SetActive(false);
        welcomeTick.SetActive(true);
        
        GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcWelcomeText.gameObject.SetActive(false);
        GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().initialStatementText.gameObject.SetActive(true);
    }

    public void OnQuestionSelected(int questionIndex)
    {
        if (questionIndex == 1)
        {
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerOneText.gameObject.SetActive(true);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerTwoText.gameObject.SetActive(false);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerThreeText.gameObject.SetActive(false);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcClarifiedResponse.gameObject.SetActive(false);
            
            question1Asked = true;
        }
        else if (questionIndex == 2)
        {
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerOneText.gameObject.SetActive(false);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerTwoText.gameObject.SetActive(true);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerThreeText.gameObject.SetActive(false);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcClarifiedResponse.gameObject.SetActive(false);
            
            question2Asked = true;
        }
        else if (questionIndex == 3)
        {
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerOneText.gameObject.SetActive(false);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerTwoText.gameObject.SetActive(false);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerThreeText.gameObject.SetActive(true);
            GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcClarifiedResponse.gameObject.SetActive(false);
            
            question3Asked = true;
            
            questionPanel.SetActive(false);
            responsePanel.SetActive(true);
        }

        CheckAllQuestionsAsked();
    }

    void CheckAllQuestionsAsked()
    {
        if (question1Asked && question2Asked && question3Asked)
        {
            situationTick.SetActive(true);
        }
    }

    public void OnResponseSelected()
    {
        questionPanel.SetActive(true);
        responsePanel.SetActive(false);
        serviceTick.SetActive(true);
        
        GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerOneText.gameObject.SetActive(false);
        GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerTwoText.gameObject.SetActive(false);
        GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcAnswerThreeText.gameObject.SetActive(false);
        GameManager.instance.currentNPC.GetComponent<NpcMovementRework>().npcClarifiedResponse.gameObject.SetActive(true);
    }
}
