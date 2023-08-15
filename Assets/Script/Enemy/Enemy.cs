using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class Enemy : MonoBehaviour
{
    //Set active only when it is in camera view //this should help to optimize the game
    //attackRange, viSionRadius, followRadius, Collider
    /*
    [SerializeField] float lightRangeOffset = 0;
    private NavMeshAgent agent;
    [SerializeField] float hitPoint;
    [SerializeField] float damageInterval = 1f;


    PlayerController chaseTarget = null;
    Mover myMover;
    bool isChasing;
    Vector3 origin;

    void Awake()
    {
        myMover = GetComponent<Mover>();
        origin = transform.position;
    }


    void Update()
    {
        if (isChasing) return;
        Return();
    }
    /// <summary>
    /// Follows the player if meets the requirement which is they need to be within the chase/follow range. 
    /// </summary>
    void Chase()
    {
        isChasing = true;
        myMover.MoveTo(chaseTarget.transform.position);
    }


    /// <summary>
    /// Gets return to it's original position
    /// </summary>
    void Return()
    {
        myMover.MoveTo(origin); //Origin is a vector3, gameObject's origin point
    }

    //Detection of player 

    void OnTriggerEnter(Collider collider)
    {
        Detection(collider);
    }
    void OnTriggerStay(Collider collider)
    {
        Detection(collider);
    }

    /// <summary>
    /// It is a helper class to detect if player in range; 
    /// </summary>
    void Detection(Collider _collider)
    {
        if (!(_collider.gameObject.CompareTag(Tag.PLAYER))) return;

        chaseTarget = _collider.gameObject.GetComponent<PlayerController>();
        var distance = Vector3.Distance(transform.position, chaseTarget.transform.position);
        if (!(distance <= chaseTarget.GetLightRange() + lightRangeOffset))
        {
            isChasing = false;
            return;
        }
        Chase();

    }
    IEnumerator DrainLight()
    {
        while (isChasing)
        {
            yield return new WaitForSeconds(damageInterval);
            //Damage logic 
            chaseTarget.playerCurrentHealth -= hitPoint;
            print("Player Health" + chaseTarget.playerCurrentHealth);
        }
    }
*/



}