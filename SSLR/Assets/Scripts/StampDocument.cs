using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDocument : MonoBehaviour
{
    [SerializeField] private GameObject financialStamp;
    [SerializeField] private GameObject residentialStamp;
    [SerializeField] private GameObject dvStamp;
    [SerializeField] private GameObject disabilitiesStamp;
    [SerializeField] private GameObject signature;

    public bool isStamped;
    public bool isSigned;
    public string assignedService;
    
    void Start()
    {
        financialStamp.SetActive(false);
        residentialStamp.SetActive(false);
        dvStamp.SetActive(false);
        disabilitiesStamp.SetActive(false);
        signature.SetActive(false);
        
        isSigned = false;
        isStamped = false;
    }
    
    public void StampFinancial()
    {
        financialStamp.SetActive(true);
        isStamped = true;
        assignedService = "Financial";
    }
    
    public void StampResidential()
    {
        residentialStamp.SetActive(true);
        isStamped = true;
        assignedService = "Residential";
    }
    
    public void StampDV()
    {
        dvStamp.SetActive(true);
        isStamped = true;
        assignedService = "Domestic Violence";
    }
    
    public void StampDisabilities()
    {
        disabilitiesStamp.SetActive(true);
        isStamped = true;
        assignedService = "Disabilities";
    }

    public void SignDocument()
    {
        signature.SetActive(true);
        isSigned = true;
    }
}
