/*
 * Author: Lin Hengrui Ryan
 * Date: 3/2/25
 * Description:
 * Customer walking handling using NavMesh
 */


using System;
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
    public Transform[] chairPosition;
    

    public void Update()
    {
        if (agent.velocity.magnitude > 0.1)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        
   
    }

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
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

    public void TakeASeat()
    {
        
    }

    public void GoToSeat()
    {
        var i = UnityEngine.Random.Range(0, chairPosition.Length);
        agent.SetDestination(chairPosition[i].position);
    }

    

}