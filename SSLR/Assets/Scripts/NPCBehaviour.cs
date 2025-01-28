/*
 * Author: Livinia Poo
 * Date: 29/1/25
 * Description: 
 * Customer behaviour
 */


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private NPCMovement npcMoveScript;
    [SerializeField] private GameObject paperObject;
    [SerializeField] private GameObject navigationTxtBubble; 
    [SerializeField] TextMeshProUGUI navigationTxt;

    private string[] possibleSentences =
    {
        "I want to go left!",
        "I want to go right!"
    };

    void Start()
    {
        npcMoveScript = GetComponent<NPCMovement>();
        
        paperObject.SetActive(false);
        navigationTxtBubble.SetActive(false);
        
        if (navigationTxt != null)
        {
            navigationTxt.text = GetRandomSentence();
        }
    }
    
    void Update()
    {
        if (npcMoveScript.inFrontOfPlayer == true)
        {
            //paperObject.SetActive(true);
            navigationTxtBubble.SetActive(true);
        }
    }

    string GetRandomSentence()
    {
        int n = Random.Range(0, possibleSentences.Length);
        return possibleSentences[n];
    }
}
