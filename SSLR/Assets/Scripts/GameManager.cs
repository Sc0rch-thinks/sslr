/*
 * Author: Livinia Poo
 * Date: 24/1/25
 * Description: 
 * Game Manager
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Assign GM instance
    /// </summary>
    public static GameManager instance;
    
    /// <summary>
    /// References and Variables
    /// </summary>
    [SerializeField]
    public bool dayEnded = false;
    
    [SerializeField]
    public bool shiftStarted;
    
    [Header("Walk Points (FOR DEBUGGING")]
    public Transform[] frontWalkPoints;
    public Transform[] leftWalkPoints;
    public Transform[] rightWalkPoints;
    
    public GameObject leftWalkPointSet;
    public GameObject rightWalkPointSet;

    /// <summary>
    /// Do Not Destroy on Load
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }   
}
