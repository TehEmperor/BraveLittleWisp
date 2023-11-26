using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Everburn : MonoBehaviour
{
    [SerializeField] float overchargeIntensity = 1f;
    [SerializeField] ChargeCrystal[] crystals;
    [SerializeField] float timeToDimTheLights = 2f;
    [SerializeField] GameObject toDim;
    PlayerController player;
    bool burning = true;
    ChargerNBurner[] burners = null;
    private void Awake() 
    {
        burners = FindObjectsOfType<ChargerNBurner>();
        player = FindObjectOfType<PlayerController>();       
        crystals = FindObjectsOfType<ChargeCrystal>();
        SetCrystalsTarget(); 
        SetBurnerParametres(burning, overchargeIntensity);
    }

    private void SetBurnerParametres(bool isBurning, float oci)
    {
        foreach (ChargerNBurner burner in burners)
        {
            burner.SetParametres(isBurning, oci);
        }

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
        SetBurnerParametres(burning, 0);
    }

    private void HushAnimation()
    {
        StartCoroutine(DimLights());        
        GetComponent<Animation>().Play();
    }

    IEnumerator DimLights()
    {
        float time = 0;
        while(time<timeToDimTheLights)
        {
           Color colour = GetComponent<Renderer>().material.GetColor("_EmissionColor");
           GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(colour, Color.black, time / timeToDimTheLights));
           Color color = toDim.GetComponent<Renderer>().material.GetColor("_EmissionColor");
           toDim.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(color, Color.black, time / timeToDimTheLights));
           Color.Lerp(RenderSettings.ambientLight, Color.black, time/timeToDimTheLights);
           time+=Time.deltaTime;
           yield return null;
        }
        RenderSettings.ambientLight = Color.black;

    }
    //AnimationEvent
    void DissolveTriger()
    {
        GetComponent<Renderer>().material.SetFloat("_DissolveTrigger", Time.time);
    } 
}
