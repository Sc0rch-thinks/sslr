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
    /*public Transform[] frontWalkPoints;
    public Transform[] leftWalkPoints;
    public Transform[] rightWalkPoints;*/
    public float movementSpeed = 10.0f;
    public float turnSpeed = 5.0f;
    private int currentPtIndex = 0;

    /*[SerializeField] private GameObject leftWalkPointSet;
    [SerializeField] private GameObject rightWalkPointSet;*/

    public bool inFrontOfPlayer = false;

    private GameManager gm;
   
    /// <summary>
    /// Start the coroutine
    /// </summary>
    void Start()
    {
        gm = GameManager.instance;
        
        StartCoroutine(WalkingToPlayer());
    }
    
    ///<summary>
    ///
    /// </summary>
    public void WalkToPlayerLeft()
    {
        gm.leftWalkPointSet.SetActive(true);
        StartCoroutine(WalkingToPlayerLeft());
    }
    
    public void WalkToPlayerRight()
    {
        gm.rightWalkPointSet.SetActive(true);
        StartCoroutine(WalkingToPlayerRight());
    }

    /// <summary>
    /// Coroutine for NPC to walk to each point
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkingToPlayer()
    {
        while (currentPtIndex < gm.frontWalkPoints.Length)
        {
            if (gm.frontWalkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = gm.frontWalkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                currentPtIndex++;

                if (currentPtIndex >= gm.frontWalkPoints.Length)
                {
                    StopAllCoroutines();
                    currentPtIndex = 0;
                    inFrontOfPlayer = true;
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
        while (currentPtIndex < gm.leftWalkPoints.Length)
        {
            if (gm.leftWalkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = gm.leftWalkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                currentPtIndex++;

                if (currentPtIndex >= gm.leftWalkPoints.Length)
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
        while (currentPtIndex < gm.rightWalkPoints.Length)
        {
            if (gm.rightWalkPoints.Length == 0)
            {
                yield break;
            }
            
            Transform targetPt = gm.rightWalkPoints[currentPtIndex];
            
            Vector3 direction = targetPt.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPt.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPt.position) < 0.1f)
            {
                currentPtIndex++;

                if (currentPtIndex >= gm.rightWalkPoints.Length)
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
