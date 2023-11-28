using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSkull : MonoBehaviour
{
    PlayerLanternFollow myFollowScript;
    Health carrier;
    [SerializeField] PlayerLanternFollow otherLantern;
    [SerializeField] GameObject target; 
    [SerializeField] float tolerance = 3f;
    [SerializeField] float dmgrate = 1f;
    public event Action onBrazerReached;
    
    private void Start() 
    {
        myFollowScript = GetComponent<PlayerLanternFollow>();
        myFollowScript.followPoint = otherLantern.followPoint;
        myFollowScript.enabled = false;        
    }

    private void Update() 
    {
        if(carrier)
        {
            carrier.GetDamaged(dmgrate*Time.deltaTime);
        }
        if(Vector3.Distance(transform.position, target.transform.position) <= tolerance)
        {
            carrier = null;
            myFollowScript.enabled = false;
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {        
        if(other.gameObject == target)
        {
            otherLantern.enabled=true;
            onBrazerReached?.Invoke();
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other) 
    {
    
     if(!other.gameObject.CompareTag("Player")) return;
     carrier = other.gameObject.GetComponent<Health>();
     myFollowScript.enabled = true;
     myFollowScript.myThrow = other.gameObject.GetComponentInChildren<Throw>();
     otherLantern.enabled = false;
    }


}
