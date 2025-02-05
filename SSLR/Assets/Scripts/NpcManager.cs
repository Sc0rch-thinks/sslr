/*
 * Author: Lin Hengrui Ryan
 * Date: 1/2/25
 * Description:
 * Npc Manager
 */

using System;
using UnityEngine;

using System.Collections.Generic;

using UnityEngine.Serialization;

public class NpcManager : MonoBehaviour
{


    /// <summary>
    /// Assign Npc Manager instance
    /// </summary>
    public static NpcManager instance;

    /// <summary>
    /// a list of all the male npcs
    /// </summary>
    public GameObject[] maleNpcs;

    /// <summary>
    /// a list of all female npcs
    /// </summary>
    public GameObject[] femaleNpcs;

    /// <summary>
    /// a prefab of the police npc
    /// </summary>
    public GameObject policeNpc;

    /// <summary>
    /// list of all the spawn points
    /// </summary>
    public Transform[] spawnPoints;

    /// <summary>
    /// a bool to check if the player is free
    /// </summary>
    public bool playerFree = false;
    
    /// <summary>
    /// collection of all exiting npcs
    /// </summary>
    public GameObject[] currentNpcs;
    
    /// <summary>
    /// a collection of positions for the chairs to sit for the npcs
    /// </summary>
    public GameObject[] chairPositions;
    /// <summary>
    /// a collection of positions for the npcs to despawn
    /// </summary>
    public Transform[] despawnPoints;

    public void spawnNpc(bool isFemale, Transform spawnPoint)
    {
        var randomNpc=0;
        if (isFemale)
        {
            randomNpc = UnityEngine.Random.Range(0, femaleNpcs.Length);
        }
        else
        {
            randomNpc = UnityEngine.Random.Range(0, maleNpcs.Length);
        }
        var npc = Instantiate(isFemale ? femaleNpcs[randomNpc] : maleNpcs[randomNpc], spawnPoint.position, Quaternion.identity);
        // currentNpcs.SetValue(npc,currentNpcs.Length);
    }

    public void Awake()
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
        
    }
}
