using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flameable : MonoBehaviour
{
    [SerializeField] float overchargeAmount = 100f;
    bool burning = false;
    [SerializeField] float burningDuration = 5f;
    float burningFor = 0f;
   private void OnCollisionEnter(Collision other) 
   {
    if(burning) return;
    if(other.gameObject.CompareTag("Flameable"))
    {
        burning = true;
        Destroy(other.gameObject);
    }    
   }
   private void Update()
   {
    if(!burning) return;
    burningFor+=Time.deltaTime;
    if(burningFor>burningDuration)
    {
        //instead of destory a charred tree stump should be left.
        Destroy(gameObject);
    }
   }
   
   private void OnTriggerStay(Collider other) 
   {
    if(!burning) return;
    if(other.gameObject.CompareTag("Player"))
    {
        other.gameObject.GetComponent<Health>().OverchargeHealth(overchargeAmount *Time.deltaTime);
    }
   }
    private void OnTriggerEnter(Collider other)
    {
        if(!burning) return;
        if (other.gameObject.CompareTag("Enemie"))
        {
            other.gameObject.GetComponent<Enemie>().BurnAndDie();
        }
    }

  
}
