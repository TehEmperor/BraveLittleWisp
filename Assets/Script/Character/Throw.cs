
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Throw : MonoBehaviour
{
    [SerializeField] PlayerLanternFollow myLantern;
    [SerializeField] Transform lightFollowPoint;
    [SerializeField] Flash flashPrefab;
    [SerializeField] float minHealthToUse = 35f;
    [SerializeField] float healthCost = 25f;
    Flash flash;
    Vector3 targetPos;
    float timeOfLastThrow;
    [SerializeField] float throwDistance = 10f;
    [SerializeField] bool isThrowToggleOn;
    [SerializeField] float coolDownPeriod = 2f;
    [Range(0.5f, 1f)]
    [SerializeField] float lightRangeMod;
    PlayerController playerController;
    Health playerHealth;

    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag(Tag.PLAYER).GetComponent<PlayerController>();  
        timeOfLastThrow = Time.time;
        playerHealth = GameObject.FindGameObjectWithTag(Tag.PLAYER).GetComponent<Health>();
        if(myLantern)
        {
        myLantern.myThrow = this;
        myLantern.followPoint = lightFollowPoint;
        }
    }

   
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse2) && isThrowToggleOn)
        {
            ThrowFlash(GetTarget(), CoolDown(), EnoughHealth());
        }
        if (Input.GetKey(KeyCode.Mouse1) && isThrowToggleOn)
        {
            Debug.DrawLine(transform.position, GetTarget(), Color.red);
        }
    }
    

    void DrawRay(Vector3 _target, Color _color)
    {
        Debug.DrawLine(transform.position, _target, _color);
    }

    void DoDamage()
    {
        playerHealth.GetDamaged(healthCost);
    }


    Vector3 GetTarget()
    {
        Vector3 target;
        
        if (!playerController.RaycastNavMesh(out target)) return Vector3.zero;
        if (Vector3.Distance(target, transform.position) <= throwDistance)
        {            
            return target;
        }
        return Vector3.zero;
    }
    void ThrowFlash(Vector3 target, bool isOnCooldown, bool isEnoughHealth)
    {
        if(!isEnoughHealth) return;       
        if(isOnCooldown) return;
        if(target == Vector3.zero) return;
        playerController.SetThrowTrigger();    
        DoDamage();
        flash = Instantiate(flashPrefab, transform.position, Quaternion.identity);
        flash.SetTarget(target);
        flash.lightRadius = playerController.GetLightRange() * lightRangeMod;   
        timeOfLastThrow = Time.time;    
        
    }

    public bool CoolDown()
    {
        if(Time.time - timeOfLastThrow < coolDownPeriod) return true;
        return false;        
    }
    bool EnoughHealth()
    {
        var healthAftherThrow = (playerHealth.GetLightIntesityModifier() * 100f) - healthCost;
        if (healthAftherThrow >= minHealthToUse)
        {
            return true;
        }
        return false;
    }


   
}
