using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyreController : MonoBehaviour
{  
    [SerializeField] float overchargeIntensity = 1f;
    [SerializeField] GameObject postProcess;
    [SerializeField] Color newEnvironmentColor;
    PlayerController player;
    Portal myPortal;
    BurningSkull[] myScull;
    Enemie[] enemiesPresent;
    [SerializeField] ParticleSystem[] pyreEffects;
    int burnedOfferings = 0;
    

    private void Start() 
    {
        InitializeLevel();        
    }   

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {player.GetComponent<Health>().OverchargeHealth(overchargeIntensity * Time.deltaTime);}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemie"))
        {
            other.gameObject.GetComponent<Enemie>().BurnAndDie();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        SecretConditionMet();
    }

    private void SecretConditionMet()
    {
        foreach(var enemy in enemiesPresent)
        {
            if(enemy == null) continue;
            return;
        }
        myPortal.ActivateSecret(true);
    }

    private void InitializeLevel()
    {
        player = FindObjectOfType<PlayerController>();   
        enemiesPresent = FindObjectsOfType<Enemie>();     
        myPortal = FindObjectOfType<Portal>();
        myScull = FindObjectsOfType<BurningSkull>();
        foreach (var scull in myScull)
        {
            scull.onBrazerReached+=BurnCounter;            
        }
        FXControl(false);
    }

    private void FXControl(bool active)
    {
        foreach (var item in pyreEffects)
        {
            if(active)
            {
                item.Play();
                continue;
            }
            else item.Stop();
        }
    }

    private void BurnCounter()
    {
        burnedOfferings+=1;
        if(burnedOfferings>= myScull.Length)
        {
            Burn();
        }
    }

    private void Burn()
    {
        FXControl(true);
        RenderSettings.ambientLight = newEnvironmentColor;
        GetComponent<Collider>().enabled = true;
        postProcess.SetActive(true);
    }

    private void OnDisable() {
        myPortal.onAllSoulsCollected-=Burn;
    }
    

}
