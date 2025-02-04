using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    [SerializeField] private float npcBufferTime;
    public bool npcSpawned = false;
    /*[SerializeField]
    List<GameObject> NPCs = new List<GameObject>();*/
    
    [SerializeField]
    GameObject npc;

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
