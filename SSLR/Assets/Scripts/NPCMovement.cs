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
    public Transform[] walkPoints1;
    public Transform[] walkPoints2;
    public Transform[] walkPoints3;
    public float movementSpeed = 10.0f;
    public float turnSpeed = 5.0f;
    private int currentPtIndex = 0;
    
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
        StartCoroutine(WalkingToPlayerLeft());
    }
    
    public void WalkToPlayerRight()
    {
        StartCoroutine(WalkingToPlayerRight());
    }

    /// <summary>
    /// Coroutine for NPC to walk to each point
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkingToPlayer()
    {
        while (currentPtIndex < walkPoints1.Length)
        {
            if (walkPoints1.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = walkPoints1[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= walkPoints1.Length)
                {
                    StopAllCoroutines();
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
        while (currentPtIndex < walkPoints2.Length)
        {
            if (walkPoints2.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = walkPoints2[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= walkPoints2.Length)
                {
                    StopAllCoroutines();
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
        while (currentPtIndex < walkPoints3.Length)
        {
            if (walkPoints3.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = walkPoints3[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                /*Debug.Log($"Reached {currentPtIndex}");*/
                currentPtIndex++;

                if (currentPtIndex >= walkPoints3.Length)
                {
                    StopAllCoroutines();
                    yield break;
                }
            }
            
            yield return null;
        }
    }
}
