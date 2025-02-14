/*
 * Author: Lin Hengrui Ryan and Livinia Poo
 * Date: 1/2/25
 * Description:
 * Npc Manager
 */

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using UnityEngine.Serialization;

public class NpcManager : MonoBehaviour
{
    /// <summary>
    /// Assign Npc Manager instance
    /// </summary>
    public static NpcManager instance;
    
    /// <summary>
    /// Assign GameManager script
    /// </summary>
    private GameManager gm;

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
    /// float for time between npc spawns
    /// </summary>
    [SerializeField] private float npcBufferTime;

    /// <summary>
    /// a bool to check if the player is free
    /// </summary>
    public bool playerFree = false;

    /// <summary>
    /// collection of all exiting npcs
    /// </summary>
    public List<GameObject> currentNpcs;    
    /// <summary>
    /// a collection of positions for the chairs to sit for the npcs
    /// </summary>
    public GameObject[] chairPositions;
    /// <summary>
    /// a collection of positions for the npcs to despawn
    /// </summary>
    public Transform[] despawnPoints;

    /// <summary>
    /// flag to prevent multiple coroutines
    /// </summary>
    private bool isSpawning = false;
    
    void Awake()
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
        
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    private void Update()
    {
        if (gm.shiftStarted)
        {
            if (currentNpcs.Count < 6 && !isSpawning)
            {
                StartCoroutine(SpawnNPCAfterWait());
            }
        }
        else if (!gm.shiftStarted)
        {
            StopAllCoroutines();
        }
    }

    public void SpawnNPC()
    {
        var randomNpc=0;
        bool isFemale = UnityEngine.Random.value > 0.5f;
        var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        if (isFemale)
        {
            randomNpc = UnityEngine.Random.Range(0, femaleNpcs.Length);
        }
        else
        {
            randomNpc = UnityEngine.Random.Range(0, maleNpcs.Length);
        }
        var npc = Instantiate(isFemale ? femaleNpcs[randomNpc] : maleNpcs[randomNpc], spawnPoint.position, Quaternion.identity);
        currentNpcs.Add(npc);
        Debug.Log($"NPC Spawned! Total NPCS: {currentNpcs.Count}");
    }

    IEnumerator SpawnNPCAfterWait()
    {
        isSpawning = true;
        yield return new WaitForSeconds(npcBufferTime);
        
        if(currentNpcs.Count < 6)
        {
            SpawnNPC();
        }
        isSpawning = false;
    }
}
