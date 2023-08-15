
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Throw : MonoBehaviour
{
    [SerializeField] Flash flashPrefab;
    [SerializeField] float minHealthToUse = 35f;
    [SerializeField] float healthCost = 25f;
    Flash flash;
    Vector3 targetPos;
    float timeOfLastThrow;
    [SerializeField] float throwDistance = 10f;
    [SerializeField] bool isThrowToggleOn;
    [SerializeField] float coolDownPeriod = 2f;
    PlayerController playerController;
    Health playerHealth;
    [SerializeField] bool isConditionMate, isReloadTimeOver, isEnemyInSight = false;
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag(Tag.PLAYER).GetComponent<PlayerController>();  
        timeOfLastThrow = Time.time;
        playerHealth = GameObject.FindGameObjectWithTag(Tag.PLAYER).GetComponent<Health>();

    }
   
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && isThrowToggleOn)
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
        DoDamage();
        flash = Instantiate(flashPrefab, transform.position, Quaternion.identity);
        flash.SetTarget(target);
        flash.lightRadius = playerController.GetLightRange();
        
    }

    bool CoolDown()
    {
        if(Time.time - timeOfLastThrow < coolDownPeriod) return true;
        timeOfLastThrow = Time.time;
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


    // public void ReloadBool(bool _bool)
    // {
    //     isReloadTimeOver = _bool;
    // }

    // // void PreProcessor()
    // // {
    // //    // Vector3 direction = (targetPos - parent.transform.position).normalized;
    // //     //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    // //     //parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, lookRotation, Time.deltaTime * 1);

    // //     float? angle = RotateTurret();

    // //     if (angle != null)// && Vector3.Angle(direction, parent.transform.forward) < 10) // When the angle is less than 10 degrees...
    // //         DoDamage(); // ...start firing.
    // // }

    // // float? RotateTurret()
    // // {
    // //     float? angle = CalculateAngle(true); // Set to false for upper range of angles, true for lower range.

    // //     if (angle != null) // If we did actually get an angle...
    // //     {
    // //         //parent.transform.LookAt(targetPos);
    // //         //this.transform.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f); // ... rotate the turret using EulerAngles because they allow you to set angles around x, y & z.
    // //     }
    // //     return angle;
    // // }
    // // float? CalculateAngle(bool low)
    // // {
    // //     Vector3 targetDir = targetPos - this.transform.position;
    // //     float y = targetDir.y;
    // //     targetDir.y = 0f;
    // //     float x = targetDir.magnitude;
    // //     float gravity = 9.81f;
    // //     float sSqr = speed * speed;
    // //     float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

    // //     if (underTheSqrRoot >= 0f)
    // //     {
    // //         float root = Mathf.Sqrt(underTheSqrRoot);
    // //         float highAngle = sSqr + root;
    // //         float lowAngle = sSqr - root;

    // //         if (low)
    // //             return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
    // //         else
    // //             return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);

    // //     }
    // //     else
    // //         return null;
    // // }
}
