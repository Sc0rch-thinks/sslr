using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public static NPCSpawn instance;
    
    [SerializeField] private float npcBufferTime;
    public bool npcSpawned = false;
    
    [SerializeField]
    GameObject npc;

    void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (!npcSpawned)
        {
            StartCoroutine(SpawnNPCAfterWait());
        }
    }

    void SpawnNPC()
    {
        StopAllCoroutines();
        GameObject spawnedNPC = Instantiate(npc, transform.position, Quaternion.identity);
        npcSpawned = true;

        GameManager.instance.currentNPC = spawnedNPC;
    }

    IEnumerator SpawnNPCAfterWait()
    {
        yield return new WaitForSeconds(npcBufferTime);

        SpawnNPC();
    }
}
