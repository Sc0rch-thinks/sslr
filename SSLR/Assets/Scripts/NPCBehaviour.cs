/*
 * Author: Livinia Poo
 * Date: 29/1/25
 * Description: 
 * Customer behaviour
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private NPCMovement npcMoveScript;
    [SerializeField] private GameObject paperObject;
    [SerializeField] private GameObject navigationTxt;

    void Start()
    {
        npcMoveScript = GetComponent<NPCMovement>();
        
        paperObject.SetActive(false);
        navigationTxt.SetActive(false);
    }
    
    void Update()
    {
        if (npcMoveScript.inFrontOfPlayer == true)
        {
            paperObject.SetActive(true);
            navigationTxt.SetActive(true);
        }
    }
}
