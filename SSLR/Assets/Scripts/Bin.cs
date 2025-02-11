/*
 * Author: Livinia Poo
 * Date: 12/2/25
 * Description: 
 * Bin
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paper"))
        {
            Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log("you can't throw that!");
        }
    }
}
