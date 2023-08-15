using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseWisp : MonoBehaviour
{
    Mover myMover;
    [SerializeField] PatrolPath patrolPath;
    float timeSinceGotToWaypoint = 0;
    [SerializeField] float chaseDistance;
    [SerializeField] float dwellDuration;
    [SerializeField] float patrolSpeedModifier = 0.5f;
    [SerializeField] float chaseSpeedMod = 1f;    
    [SerializeField] float callRaius = 10f;
    [SerializeField] AudioClip discoverPlayerClip;
    [SerializeField] float slowPlayerDuration = 5f;
    Coroutine slowPlayerRoutine;
    bool isChasing = false;

    PlayerController target = null;
    Vector3 nextPosition;
    int waypointIndex = 0;
    float wayPointTolerance = 1f;

    private void Awake() 
    {
        myMover = GetComponent<Mover>();
        chaseDistance = GetComponent<SphereCollider>().radius - 2;
    }


    private void Update() 
    {
        if(FleeFromPlayerLight())return;
        if(PlayerSpotedBehaviour()) return;
        PatrolBehaviour();
        timeSinceGotToWaypoint += Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.GetComponent<PlayerController>();
        }
    }


    public bool PlayerSpotedBehaviour()
    {
        if(target == null) return false;
        if (Vector3.Distance(transform.position, target.transform.position) <= chaseDistance)
        {
            if (!isChasing) 
            {GetComponent<AudioSource>().PlayOneShot(discoverPlayerClip);
            CallNearbyEnemies();}            
            isChasing = true;
            
            if (slowPlayerRoutine == null) slowPlayerRoutine = StartCoroutine(SlowPlayer());
            myMover.MoveTo(target.transform.position, chaseSpeedMod);
            return true;
        }
        isChasing = false;
        return false;        
    }

    private bool FleeFromPlayerLight()
    {
        if (target)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= target.GetLightRange())
            {
                if(slowPlayerRoutine!=null){slowPlayerRoutine = null;}
                var fleeVector = target.transform.position - this.transform.position;
                var moveVector = this.transform.position - fleeVector;
                myMover.MoveTo(moveVector, 1.3f);
                return true;
            }
            return false;
        }
        return false;

    }

    IEnumerator SlowPlayer()
    {        
        target.GetSlowed(slowPlayerDuration);
        yield return new WaitForSeconds(1f);
    }

    private void CallNearbyEnemies()
    {      
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, callRaius, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            Enemie ai = hit.collider.gameObject.GetComponent<Enemie>();
            if (ai == null) continue;
            ai.RoamingUpdate(target.transform.position, 2);

        }
    }

    

    private void PatrolBehaviour()
    {
        if (slowPlayerRoutine != null) { slowPlayerRoutine = null; }
        if (patrolPath != null)
        {
            if (AtWayPoint())
            {
                timeSinceGotToWaypoint = 0;
                CycleWaipoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        if (timeSinceGotToWaypoint > dwellDuration)
        {
            myMover.MoveTo(nextPosition, patrolSpeedModifier);
        }

    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWayPoint(waypointIndex).position;
    }

    private void CycleWaipoint()
    {

        {

            waypointIndex = patrolPath.GetNextIndex(waypointIndex);

        }

    }

    private bool AtWayPoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < wayPointTolerance;
    }


}
