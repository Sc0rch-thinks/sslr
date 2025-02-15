/*
 * Author: Lin Hengrui Ryan
 * Date: 3/2/25
 * Description:
 * Customer walking handling using NavMesh
 */


using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovementRework : MonoBehaviour
{
    /// <summary>
    /// nav mesh agent
    /// </summary>
    public NavMeshAgent agent;

    public Animator animator;
    private static readonly int IsSitting = Animator.StringToHash("isSitting");
    private static readonly int Speed = Animator.StringToHash("Speed");


    public void Update()
    {
        if (agent.velocity.magnitude > 0.1)
        {
            animator.SetFloat(Speed, 1);
        }
        else
        {
            animator.SetFloat(Speed, 0);
        }
    }

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(SitDown());
    }

    public void Called()
    {
        StartCoroutine(CustomerCalled());
    }
    
    public IEnumerator CustomerCalled()
    {
        var pos = NpcManager.instance.desk;
        agent.SetDestination(pos.position);
        while (true)
        {
            var npcpos = gameObject.transform.position;
            npcpos.y = 0;
            var dist= Vector3.Distance(pos.position,npcpos);
            if (dist<0.5f)
            {
                agent.SetDestination(gameObject.transform.position);
                gameObject.transform.rotation = pos.transform.rotation;
                break;
                
            }

            yield return 0;
        }
    }


    public IEnumerator SitDown()
    {
        var i = Random.Range(0, NpcManager.instance.chairPositions.Length);
        var pos = NpcManager.instance.chairPositions[i];


        var sittingPosition = pos.transform.position;
        sittingPosition.y = 0;
        agent.SetDestination(sittingPosition);
        
        while (true)
        {
            var dist= Vector3.Distance(sittingPosition,gameObject.transform.position);
            // Debug.Log(dist);
            if (dist < 0.05f)
            {
                agent.SetDestination(gameObject.transform.position);
                animator.SetBool(IsSitting, true);
                gameObject.transform.rotation = pos.transform.rotation;
                break;
            }
            yield return 0;
        }
    }

    public void Despawn()
    {
        var random = Random.Range(0, NpcManager.instance.despawnPoints.Length);
        agent.SetDestination(NpcManager.instance.despawnPoints[random].position);
    }
}