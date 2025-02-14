/*
 * Author: Livinia Poo
 * Date: 22/1/25
 * Description: 
 * Customer despawns in certain area
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private Player playerScript;
    NPCSpawn npcSpawnScript;
    
    /// <summary>
    /// Destroy NPC object on trigger enter
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            GameManager.instance.currentNPC = null;
            /*
            NPCSpawn.instance.npcSpawned = false;
            */
            
            Destroy(other.gameObject);
        }
    }
}
