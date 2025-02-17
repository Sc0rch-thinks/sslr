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
    public Transform playerCam;

    void Start()
    {
        if (Camera.main != null)
        {
            playerCam = Camera.main.transform;
        }
    }

    void Update()
    {
        if (playerCam != null && Vector3.Distance(transform.position, playerCam.position) < 2)
        {
            transform.LookAt(transform.position + playerCam.forward);
        }
    }
}
