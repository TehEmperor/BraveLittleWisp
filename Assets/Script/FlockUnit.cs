using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    [SerializeField] float FOVAngle;
    private List<FlockUnit> cohesionNeghbors = new List<FlockUnit>();
    private Flock assignedFlock;
    public Transform myTransform {get;set;}
    private void Awake() {
        myTransform = transform; 
    }
    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock; 
    }

    public void MoveUnit()
    {
        FindNeighboringUnits();
        Vector3 cohesionVector = ClculateCohesionVector();
    }

    private void FindNeighboringUnits()
    {
        cohesionNeghbors.Clear();
        var allUnits = assignedFlock.allUnits;
        for (int i = 0; i < allUnits.Length; i++)
        {
            var currentUnit = allUnits[i];
            if(currentUnit == this) continue;
            float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.myTransform.position - myTransform.position); 
            if(currentNeighbourDistanceSqr >= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance) return; 
            cohesionNeghbors.Add(currentUnit);
        }

    }

    private Vector3 ClculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;
        int neighborsInFov = 0;
        if(cohesionNeghbors.Count ==0) {return cohesionVector;}
        for (int i = 0; i < cohesionNeghbors.Count; i++)
        {
            if(!IsInFov(cohesionNeghbors[i].myTransform.position)) continue;
            neighborsInFov++;
            cohesionVector += cohesionNeghbors[i].myTransform.position;
            
        }
        if (neighborsInFov == 0) return cohesionVector;
        cohesionVector /= neighborsInFov;
        cohesionVector -= myTransform.position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;

    }

    private bool IsInFov(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
    }
}
