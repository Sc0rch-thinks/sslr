/*
 * Author: Livinia Poo
 * Date: 12/2/25
 * Description: 
 * Spawning paper in area
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class PaperSpawn : MonoBehaviour
{
    /// <summary>
    /// What to spawn
    /// </summary>
    [SerializeField] private GameObject paperPrefab;
    
    /// <summary>
    /// Player's hands/Controllers
    /// </summary>
    private XRDirectInteractor playerHand;
    private bool handInPaperSpawn = false;

    private InputDevice targetDevice;

    /// <summary>
    /// Assigning hand
    /// </summary>
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        else
        {
            Debug.LogWarning("No XR Controller detected!");
        }
    }
    
    /// <summary>
    /// Detecting player hand
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            handInPaperSpawn = true;
            playerHand = other.GetComponent<XRDirectInteractor>();
        }
    }

    /// <summary>
    /// Detecting player hand leave area
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            handInPaperSpawn = false;
            playerHand = null;
        }
    }

    /// <summary>
    /// Spawn paper if in trigger and grabbing
    /// </summary>
    void Update()
    {
        if (handInPaperSpawn && playerHand != null && !playerHand.hasSelection)
        {
            bool isGrabbing = false;
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out isGrabbing);
            
            if (isGrabbing)
            {
                SpawnPaper();
            }
        }
    }

    /// <summary>
    /// Instantiate paper object
    /// </summary>
    void SpawnPaper()
    {
        GameObject spawnedPaper = Instantiate(paperPrefab, playerHand.transform.position, Quaternion.identity);
        XRGrabInteractable grabComponent = spawnedPaper.GetComponent<XRGrabInteractable>();
        
        if (grabComponent != null)
        {
            playerHand.interactionManager.SelectEnter((IXRSelectInteractor)playerHand, (IXRSelectInteractable)grabComponent);
        }
    }
}
