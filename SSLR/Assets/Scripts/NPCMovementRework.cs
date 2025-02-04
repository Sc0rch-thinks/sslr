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

    public Transform desk;
    public float walkRange;
    public LayerMask waitingArea;
    public LayerMask despawnArea;
    public Vector3 roamingPoint;
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
        var sitCoro = SitDown();
        StartCoroutine(sitCoro);
    }


    public void CustomerCalled()
    {
        agent.SetDestination(desk.position);
    }

    public void Wait()
    {
        Debug.Log("walking");
        agent.SetDestination(roamingPoint);
        
    }

    public IEnumerator SitDown()
    {
        var i = Random.Range(0, NpcManager.instance.chairPositions.Length);
        Debug.Log(i);


        var pos = NpcManager.instance.chairPositions[i];

        agent.SetDestination(pos.transform.position);

        var sittingPosition = pos.transform.position+new Vector3(0,0.5f,0);
        while (true)
        {
            var dist= Vector3.Distance(pos.transform.position,gameObject.transform.position);
            if (dist <0.5)
            {
                agent.SetDestination(gameObject.transform.position);
                gameObject.transform.position=sittingPosition;
                gameObject.transform.rotation = pos.transform.rotation;
                animator.SetBool(IsSitting,true);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    

    

}