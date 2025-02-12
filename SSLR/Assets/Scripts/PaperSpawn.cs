using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class PaperSpawn : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    private XRDirectInteractor playerHand;
    private bool handInPaperSpawn = false;

    private InputDevice targetDevice;

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
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            handInPaperSpawn = true;
            playerHand = other.GetComponent<XRDirectInteractor>();
            
            Debug.Log("Player hand in paper area!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            handInPaperSpawn = false;
            playerHand = null;
            
            Debug.Log("Player hand left paper area!");
        }
    }

    void Update()
    {
        if (handInPaperSpawn && playerHand != null && !playerHand.hasSelection)
        {
            bool isGrabbing = false;
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out isGrabbing);
            
            if (isGrabbing)
            {
                Debug.Log("Player hand grabbing in paper area!");
                SpawnPaper();
            }
        }
    }

    void SpawnPaper()
    {
        GameObject spawnedPaper = Instantiate(paperPrefab, playerHand.transform.position, Quaternion.identity);
        XRGrabInteractable grabComponent = spawnedPaper.GetComponent<XRGrabInteractable>();
        
        Debug.Log("Paper Spawned!");

        if (grabComponent != null)
        {
            playerHand.interactionManager.SelectEnter((IXRSelectInteractor)playerHand, (IXRSelectInteractable)grabComponent);
        }
    }
}
