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

    public bool isStamped;
    public bool isSigned;
    public string assignedDepartment;
    public string assignedService;
    
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
    
    public void StampFinancial()
    {
        financialStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Financial";
    }
    
    public void StampResidential()
    {
        residentialStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Residential";
    }
    
    public void StampDV()
    {
        dvStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Domestic Violence";
    }
    
    public void StampDisabilities()
    {
        disabilitiesStamp.SetActive(true);
        isStamped = true;
        assignedDepartment = "Disabilities";
    }

    public void SignComCare()
    {
        comcareSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "ComCare";
    }
    
    public void SignFSC()
    {
        fscSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "Family Service Centres";
    }
    
    public void SignPEERS()
    {
        peersSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "PEERS";
    }
    
    public void SignTransitionalShelters()
    {
        transitionalShelterSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "Transitional Shelters";
    }
    
    public void SignCPS()
    {
        cpsSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "CPS";
    }
    
    public void SignChildrenYoungHome()
    {
        childrenYoungHomeSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "Children and Young Persons Homes";
    }

    public void SignSGEnable()
    {
        sgEnableSignature.SetActive(true);
        isSigned = true;
        assignedDepartment = "SG Enable";
    }
}
