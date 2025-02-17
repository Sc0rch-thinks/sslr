/*
 * Author: Lin Hengrui Ryan, Livinia Poo
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
    /// List of seats for NPCs in scene
    /// </summary>
    public Seat[] Seats;
    
    /// <summary>
    /// collection of all exiting npcs
    /// </summary>
    public List<GameObject> currentNpcs;

    /// <summary>
    /// a collection of positions for the npcs to despawn
    /// </summary>
    public Transform[] despawnPoints;

    /// <summary>
    /// flag to prevent multiple coroutines
    /// </summary>
    private bool isSpawning = false;

    /// <summary>
    /// the position of the desk
    /// </summary>
    public Transform desk;

    /// <summary>
    /// singleton pattern
    /// </summary>
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

    /// <summary>
    /// called once per frame to check if the shift has started and if there are less than 4 npcs
    /// </summary>
    private void Update()
    {
        if (gm.shiftStarted)
        {
            if (currentNpcs.Count < 4 && !isSpawning)
            {
                StartCoroutine(SpawnNPCAfterWait());
            }
        }
        else if (!gm.shiftStarted)
        {
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// to spawn a new npc
    /// </summary>
    public void SpawnNPC()
    {
        var randomNpc = 0;
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

        var npc = Instantiate(isFemale ? femaleNpcs[randomNpc] : maleNpcs[randomNpc], spawnPoint.position,
            Quaternion.identity);
        currentNpcs.Add(npc);
        Debug.Log($"NPC Spawned! Total NPCS: {currentNpcs.Count}");
    }

    /// <summary>
    /// to spawn a new npc after a delay
    /// </summary>
    IEnumerator SpawnNPCAfterWait()
    {
        isSpawning = true;
        yield return new WaitForSeconds(npcBufferTime);

        if (currentNpcs.Count < 4)
        {
            SpawnNPC();
        }

        isSpawning = false;
    }

    /// <summary>
    /// to end the day and despawn all npcs
    /// </summary>
    public void EndDay()
    {
        foreach (var npc in currentNpcs)
        {
            npc.GetComponent<NpcMovementRework>().Despawn(true);
        }
    }

    /// <summary>
    /// a struct to store the seat object and availability
    /// </summary>
    [Serializable]
    public struct Seat
    {
        public GameObject SeatObject;
        public bool Available;
    }
}