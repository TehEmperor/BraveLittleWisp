using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] FlockUnit flockingUnitPrefab;
    [SerializeField] int flockSize;
    [SerializeField] Vector3 spawnBounds;

    [Header("Detection Distances")]
    [SerializeField] float _cohesionDistance;  

    public float cohesionDistance {get {return _cohesionDistance;}}

    public FlockUnit[] allUnits { get; set;}

    private void Start() {
        Generateunits();
    }

    private void Generateunits()
    {
       allUnits = new FlockUnit[flockSize];
       for (int i = 0; i < flockSize; i++)
       {
        var randomVector = UnityEngine.Random.insideUnitSphere;
        randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
        var spawnPosition = transform.position + randomVector;
        var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        allUnits[i] = Instantiate(flockingUnitPrefab, spawnPosition, rotation);
        allUnits[i].AssignFlock(this);
       }
    }
}
