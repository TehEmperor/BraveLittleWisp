using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Fish : MonoBehaviour
{
    [Header("Patroling")]
    [SerializeField] PatrolPath myPath;
    [Header("Random")]
    [SerializeField] float swimRange;
    [SerializeField] float ySwimRange;
    [SerializeField] float roamTolerance;
    [SerializeField] float dwellDuration = 1f;
    [SerializeField] float speedMod;
    int wpIndex = 0;
    float timeSinceReachedDest = 0;
    Vector3 nextDest;
    Vector3 origin;
    Mover myMover;

    // Start is called before the first frame update
    void Start()
    {   myMover = GetComponent<Mover>();
        if(myPath == null) return;
        nextDest = GenerateRoamPoint();
        origin = transform.position;
    }

    
    void Update()
    {
        Debug.DrawLine(transform.position, nextDest, Color.green, 1f);
        SwimAround();
        timeSinceReachedDest += Time.deltaTime;
                
    }

  


    private void SwimAround()
    {
        if(myPath != null)
        {
            PathSwim();
            if (timeSinceReachedDest > dwellDuration)
            {
                myMover.MoveTo(nextDest, speedMod);
            }

        }

        if(ySwimRange - transform.position.y <= roamTolerance)
        {
            nextDest = origin;
        }
        if(AtDestination(nextDest))
        {
          timeSinceReachedDest = 0;
          nextDest = GenerateRoamPoint();
        }
        if(timeSinceReachedDest > dwellDuration)
        {
            myMover.MoveTo(nextDest, speedMod);
        }

    }

    private void PathSwim()
    {
        if (AtDestination(GetCurrentWaypoint()))
        {
            timeSinceReachedDest = 0;
            CycleWaipoint();
        }
        nextDest = GetCurrentWaypoint();        
    }

    private Vector3 GetCurrentWaypoint()
    {
        return myPath.GetWayPoint(wpIndex).position;
    }

    private void CycleWaipoint()
    {
        {
            wpIndex = myPath.GetNextIndex(wpIndex);
        }

    }
    

    private bool AtDestination(Vector3 dest)
    {
        return Vector3.Distance(transform.position, dest) < roamTolerance;
    }

    private Vector3 GenerateRoamPoint()
    {
        float xMod = Random.Range(-swimRange, swimRange);
        float zMod = Random.Range(-swimRange, swimRange);
        Vector3 destination = new Vector3(transform.position.x + xMod, transform.position.y, transform.position.z + zMod);
        NavMeshHit nHit = new NavMeshHit();
        if(!NavMesh.SamplePosition(destination, out nHit, roamTolerance, NavMesh.AllAreas)) return GenerateRoamPoint();
        NavMeshPath path = new NavMeshPath();
        if(NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            return destination;
        }
        else 
        {
            return GenerateRoamPoint();
        }
        
    }
}
