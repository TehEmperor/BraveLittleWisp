using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingBlock : MonoBehaviour
{
   [SerializeField] ChargeCrystal[] crystals;
   [SerializeField] GameObject objToHide;
   bool dissolved = false;
   private void Start()
   {    
    SetCrystalsTarget();
   }

    private void SetCrystalsTarget()
    {
        foreach(ChargeCrystal crystal in crystals)
        {
            if(crystal == null) return;
            crystal.SetMeltObject(this.gameObject);
            crystal.onFullCharge+=CheckCrystals;
        }
    }

    private void CheckCrystals()
    {
        if(dissolved) return;
        foreach (var crystal in crystals)
        {
            if(!crystal.CheckCharge()) return;            
        }        
        GetComponent<Renderer>().material.SetFloat("_DissolveTrigger", Time.time);    
        GetComponent<Collider>().isTrigger = true;
        dissolved = true;
        objToHide.SetActive(true);    
    }



}
