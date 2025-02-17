/*
 * Author: Livinia Poo
 * Date: 13/2/25
 * Description: 
 * Document Logic
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDocument : MonoBehaviour
{
    [Header("Stamps")] 
    [SerializeField] private GameObject financialStamp;
    [SerializeField] private GameObject residentialStamp;
    [SerializeField] private GameObject dvStamp;
    [SerializeField] private GameObject disabilitiesStamp;
    [SerializeField] private GameObject signature;

    [Header("Signatures")] 
    [SerializeField] private GameObject comcareSignature;
    [SerializeField] private GameObject fscSignature;
    [SerializeField] private GameObject peersSignature;
    [SerializeField] private GameObject transitionalShelterSignature;
    [SerializeField] private GameObject cpsSignature;
    [SerializeField] private GameObject childrenYoungHomeSignature;
    [SerializeField] private GameObject sgEnableSignature;

    /// <summary>
    /// Details to reference later on 
    /// </summary>
    public bool isStamped;
    public bool isSigned;
    public string assignedDepartment;
    public string assignedService;
    
    /// <summary>
    /// See finalised document detail
    /// </summary>
    /// <returns></returns>
    private string CheckFinalDepartment()
    {
        return $"{assignedDepartment}-{assignedService}";
    } 
    
    /// <summary>
    /// Disabling all signatures and samps
    /// </summary>
    void Start()
    {
        financialStamp.SetActive(false);
        residentialStamp.SetActive(false);
        dvStamp.SetActive(false);
        disabilitiesStamp.SetActive(false);
        signature.SetActive(false);

        comcareSignature.SetActive(false);
        fscSignature.SetActive(false);
        peersSignature.SetActive(false);
        transitionalShelterSignature.SetActive(false);
        cpsSignature.SetActive(false);
        childrenYoungHomeSignature.SetActive(false);
        sgEnableSignature.SetActive(false);

        isSigned = false;
        isStamped = false;
    }

    /// <summary>
    /// Financial stamp appear, assign department and declare stamped
    /// </summary>
    public void StampFinancial()
    {
        financialStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Financial";
    }

    /// <summary>
    /// Residential stamp appear, assign department and declare stamped
    /// </summary>
    public void StampResidential()
    {
        residentialStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Residential";
    }

    /// <summary>
    /// DV stamp appear, assign department and declare stamped
    /// </summary>
    public void StampDV()
    {
        dvStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Domestic Violence";
    }

    /// <summary>
    /// Disabilities stamp appear, assign department and declare stamped
    /// </summary>
    public void StampDisabilities()
    {
        disabilitiesStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Disabilities";
    }

    /// <summary>
    /// Respectivee signature appear, assign servoce and declare signed
    /// </summary>
    public void Sign(string service)
    {
        switch (service)
        {
            case "ComCare":
                comcareSignature.SetActive(true);
                isSigned = true;
                assignedDepartment = "ComCare";
                PlayerDialogueInteraction.instance.servicesPanel.SetActive(false);
                PlayerDialogueInteraction.instance.questionPanel.SetActive(true);
                break;
            case "FSC":
                fscSignature.SetActive(true);
                isSigned = true;
                assignedDepartment = "Family Service Centres";
                PlayerDialogueInteraction.instance.servicesPanel.SetActive(false);
                PlayerDialogueInteraction.instance.questionPanel.SetActive(true);
                break;
            case "PEERS":
                peersSignature.SetActive(true);
                isSigned = true;
                assignedDepartment = "PEERS";
                PlayerDialogueInteraction.instance.servicesPanel.SetActive(false);
                PlayerDialogueInteraction.instance.questionPanel.SetActive(true);
                break;
            case "Transitional Shelters":
                transitionalShelterSignature.SetActive(true);
                isSigned = true;
                assignedDepartment = "Transitional Shelters";
                PlayerDialogueInteraction.instance.servicesPanel.SetActive(false);
                PlayerDialogueInteraction.instance.questionPanel.SetActive(true);
                break;
            case "CPS":
                cpsSignature.SetActive(true);
                isSigned = true;
                assignedDepartment = "CPS";
                PlayerDialogueInteraction.instance.servicesPanel.SetActive(false);
                PlayerDialogueInteraction.instance.questionPanel.SetActive(true);
                break;
            case "SG Enable":
                sgEnableSignature.SetActive(true);
                isSigned = true;
                assignedDepartment = "SG Enable";
                PlayerDialogueInteraction.instance.servicesPanel.SetActive(false);
                PlayerDialogueInteraction.instance.questionPanel.SetActive(true);
                break;
                
        }
    }


    /// <summary>
    /// Declares this is current document
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pen"))
        {
            PlayerDialogueInteraction.instance.servicesPanel.SetActive(true);
            PlayerDialogueInteraction.instance.currentDocument = this;
        }
    }
}