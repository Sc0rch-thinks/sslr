/*
 * Author: Livinia Poo
 * Date: 22/1/25
 * Description: 
 * Customer walking handling
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    /// <summary>
    /// References and Variables
    /// </summary>
    public Transform[] frontWalkPoints;
    public Transform[] leftWalkPoints;
    public Transform[] rightWalkPoints;
    public float movementSpeed = 10.0f;
    public float turnSpeed = 5.0f;
    private int currentPtIndex = 0;

    [SerializeField] private GameObject leftWalkPointSet;
    [SerializeField] private GameObject rightWalkPointSet;
    
    public GameObject paperObject;
    
    /// <summary>
    /// Start the coroutine
    /// </summary>
    void Start()
    {
        StartCoroutine(WalkingToPlayer());
    }
    
    ///<summary>
    ///
    /// </summary>
    public void WalkToPlayerLeft()
    {
        leftWalkPointSet.SetActive(true);
        StartCoroutine(WalkingToPlayerLeft());
    }
    
    public void WalkToPlayerRight()
    {
        rightWalkPointSet.SetActive(true);
        StartCoroutine(WalkingToPlayerRight());
    }

    /// <summary>
    /// Coroutine for NPC to walk to each point
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkingToPlayer()
    {
        while (currentPtIndex < frontWalkPoints.Length)
        {
            if (frontWalkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = frontWalkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= frontWalkPoints.Length)
                {
                    StopAllCoroutines();
                    currentPtIndex = 0;
                    paperObject.SetActive(true);
                    yield break;
                }
            }
            
            yield return null;
        }
    }
    
    /// <summary>
    /// Coroutine for NPC to walk to each point
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkingToPlayerLeft()
    {
        while (currentPtIndex < leftWalkPoints.Length)
        {
            if (leftWalkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = leftWalkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= leftWalkPoints.Length)
                {
                    StopAllCoroutines();
                    currentPtIndex = 0;
                    yield break;
                }
            }
            
            yield return null;
        }
    }
    
    /// <summary>
    /// Coroutine for NPC to walk to each point
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkingToPlayerRight()
    {
        while (currentPtIndex < rightWalkPoints.Length)
        {
            if (rightWalkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = rightWalkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= rightWalkPoints.Length)
                {
                    StopAllCoroutines();
                    currentPtIndex = 0;
                    yield break;
                }
            }
            
            yield return null;
        }
    }
}
