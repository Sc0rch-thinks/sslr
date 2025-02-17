/*
 * Author: Livinia Poo
 * Date: 17/2/25
 * Description: 
 * For UI Labels to Face Player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFacePlayer : MonoBehaviour
{
    /// <summary>
    /// Referencinig player's cam
    /// </summary>
    public Transform playerCam;

    /// <summary>
    /// Player's cam details
    /// </summary>
    void Start()
    {
        if (Camera.main != null)
        {
            playerCam = Camera.main.transform;
        }
    }

    /// <summary>
    /// UI will always face player
    /// </summary>
    void Update()
    {
        if (playerCam != null && Vector3.Distance(transform.position, playerCam.position) < 2)
        {
            transform.LookAt(transform.position + playerCam.forward);
        }
    }
}
