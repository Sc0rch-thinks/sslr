using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDocument : MonoBehaviour
{
    [SerializeField] private GameObject financialStamp;
    [SerializeField] private GameObject residentialStamp;
    [SerializeField] private GameObject dvStamp;
    [SerializeField] private GameObject disabilitiesStamp;

    void Awake()
    {
        financialStamp.SetActive(false);
        residentialStamp.SetActive(false);
        dvStamp.SetActive(false);
        disabilitiesStamp.SetActive(false);
    }
    
    public void StampFinancial()
    {
        financialStamp.SetActive(true);
    }
    
    public void StampResidential()
    {
        residentialStamp.SetActive(true);
    }
    
    public void StampDV()
    {
        dvStamp.SetActive(true);
    }
    
    public void StampDisabilities()
    {
        disabilitiesStamp.SetActive(true);
    }
}
