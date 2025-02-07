/*
 * Author: Livinia Poo
 * Date: 24/1/25
 * Description: 
 * Door opening for certain objects/conditions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    /// <summary>
    /// References and variables
    /// </summary>
    private GameManager gm;
    
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;
    /// <summary>
    /// a value to control delay the door closing
    /// </summary>
    public float doorCloseDelay = 0.75f;
    private bool doorOpened = false;

    /// <summary>
    /// Assigning Objects to variables
    /// </summary>
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        if (gm == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    /// <summary>
    /// Open door based on certain conditions
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!doorOpened && (other.CompareTag("Player") || other.CompareTag("NPC")))
        {
            OpenDoor();
        }
    }

    /// <summary>
    /// Rotating door meshes
    /// </summary>
    void OpenDoor()
    {
        if (doorOpened) return;
        
        Vector3 currentLeftRotation = doorLeft.transform.eulerAngles;
        currentLeftRotation.y += 90;
        doorLeft.transform.eulerAngles = currentLeftRotation;
        
        Vector3 currentRightRotation = doorRight.transform.eulerAngles;
        currentRightRotation.y -= 90;
        doorRight.transform.eulerAngles = currentRightRotation;
        
        doorOpened = true;
        StartCoroutine(CloseDoorAfterDelay(doorCloseDelay));
    }

    /// <summary>
    /// Close door after .75f delay
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Vector3 currentLeftRotation = doorLeft.transform.eulerAngles;
        currentLeftRotation.y -= 90;
        doorLeft.transform.eulerAngles = currentLeftRotation;
        
        Vector3 currentRightRotation = doorRight.transform.eulerAngles;
        currentRightRotation.y += 90;
        doorRight.transform.eulerAngles = currentRightRotation;
        
        doorOpened = false;
    }
}
