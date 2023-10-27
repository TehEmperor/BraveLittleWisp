using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UneadWatcher : MonoBehaviour
{
    [SerializeField] Transform rHand;
    [SerializeField] Transform head;
    [SerializeField] Enemie[] enemiesToAlert;
    [SerializeField] float alertInterval = 1f;
    Transform player;
    private float timeSinceLastAlert = 0f;
    private bool isAlerting;

    private void Update() 
    {
        if(!isAlerting) return;
        if(timeSinceLastAlert >= alertInterval)
        {
            AlertMinions(player.transform.position);
            timeSinceLastAlert = 0;
        }
        timeSinceLastAlert +=Time.deltaTime;

    }

    
    private void OnTriggerStay(Collider other) 
    {
        if(!other.gameObject.CompareTag("Player")) return;
        rHand.position = other.transform.position;
        head.position = other.transform.position;        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(!other.gameObject.CompareTag("Player")) return;
        player = other.transform;
        AlertMinions(player.transform.position);
        isAlerting=true;
                
    }

    private void OnTriggerExit(Collider other) 
    {
        if (!other.gameObject.CompareTag("Player")) return;
        isAlerting = false;
        
    }

    private void AlertMinions(Vector3 position)
    {
        foreach(var enemie in enemiesToAlert)
        {
            enemie.RoamingUpdate(position, 2);
        }
    }
}
