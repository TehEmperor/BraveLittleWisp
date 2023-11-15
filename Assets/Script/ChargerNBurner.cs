using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerNBurner : MonoBehaviour
{
    [SerializeField] bool burning = false;
    private float ocIntensity = 1f;

    public void SetParametres(bool isBurning, float overchargeIntensity)
    {
        burning=isBurning;
        gameObject.SetActive(isBurning);
        ocIntensity = overchargeIntensity;
    } 
   

    private void OnTriggerStay(Collider other)
    {
        if (!burning) return;
        if (other.gameObject.CompareTag("Player"))
        { other.GetComponent<Health>().OverchargeHealth(ocIntensity * Time.deltaTime); }
        if (other.gameObject.CompareTag("Enemie"))
        {

            other.gameObject.GetComponent<Enemie>().BurnAndDie();
        }
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
