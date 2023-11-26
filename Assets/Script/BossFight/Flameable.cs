using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flameable : MonoBehaviour
{
    [SerializeField] float overchargeAmount = 100f;
    bool burning = false;
    [SerializeField] float burningDuration = 5f;
    [SerializeField] GameObject trunk;
    float burningFor = 0f;
    
    private void Start() 
    {
        FireControl();                
    }

   private void OnCollisionEnter(Collision other) 
   {
    if(burning) return;
    if(other.gameObject.CompareTag("Flameable"))
    {
        burning = true;
        FireControl();
        Destroy(other.gameObject);
    }    
   }

   private void Update()
   {
    if(!burning) return;
    burningFor+=Time.deltaTime;
    if(burningFor>burningDuration)
    {
        burning = false;
        Instantiate(trunk, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
   }

   private void FireControl()
   {
    foreach (var particle in GetComponentsInChildren<ParticleSystem>())
    {
        if(particle.isEmitting) {particle.Stop(); continue;}
        else particle.Play();
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
