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
    public Transform[] walkPoints;
    public float movementSpeed = 10.0f;
    public float turnSpeed = 5.0f;
    private int currentPtIndex = 0;
    
    /// <summary>
    /// Start the coroutine
    /// </summary>
    void Start()
    {
        StartCoroutine(Walking());
    }

    /// <summary>
    /// Coroutine for NPC to walk to each point
    /// </summary>
    /// <returns></returns>
    IEnumerator Walking()
    {
        while (currentPtIndex < walkPoints.Length)
        {
            if (walkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = walkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= walkPoints.Length)
                {
                    StopAllCoroutines();
                    yield break;
                }
            }
            
            yield return null;
        }
    }
}
