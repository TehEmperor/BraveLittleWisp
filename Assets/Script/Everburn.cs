using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Everburn : MonoBehaviour
{
    [SerializeField] float overchargeIntensity = 1f;
    [SerializeField] ChargeCrystal[] crystals;
    [SerializeField] GameObject postProcess;
    PlayerController player;
    bool burning = true;

    private void Awake() 
    {
        player = FindObjectOfType<PlayerController>();       
        crystals = FindObjectsOfType<ChargeCrystal>();
        SetCrystalsTarget(); 
    }

    private void SetCrystalsTarget()
    {
        foreach (ChargeCrystal crystal in crystals)
        {
            if (crystal == null) return;
            crystal.SetMeltObject(this.gameObject);
            crystal.onFullDepletion += CheckCrystals;
        }
    }
    private void CheckCrystals()
    {
        if (!burning) return;
        foreach (var crystal in crystals)
        {
            if (!crystal.CheckDepletion()) return;
        }
        HushAnimation();
        burning = false;
    }

    private void HushAnimation()
    {        
        postProcess.SetActive(false);
        GetComponent<Animation>().Play();
    }

    //AnimationEvent
    void DissolveTriger()
    {
        GetComponent<Renderer>().material.SetFloat("_DissolveTrigger", Time.time);
    } 


    private void OnTriggerStay(Collider other)
    {
        if(!burning) return;
        if (other.gameObject.CompareTag("Player"))
        { player.GetComponent<Health>().OverchargeHealth(overchargeIntensity * Time.deltaTime); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!burning) return;
        if (other.gameObject.CompareTag("Enemie"))
        {
            other.gameObject.GetComponent<Enemie>().BurnAndDie();
        }
    }
}
