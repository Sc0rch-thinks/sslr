using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PaperSpawn : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    private XRDirectInteractor playerHand;
    private bool handInPaperSpawn = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            handInPaperSpawn = true;
            playerHand = other.GetComponent<XRDirectInteractor>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            handInPaperSpawn = false;
            playerHand = null;
        }
    }

    void Update()
    {
        if (handInPaperSpawn && playerHand != null && !playerHand.hasSelection)
        {
            if (Input.GetButtonDown("Select"))
            {
                SpawnPaper();
            }
        }
    }

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
