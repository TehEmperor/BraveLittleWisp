using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
   [SerializeField] Transform origin;
   [SerializeField] GameObject propPrefab;
   [SerializeField] int amount;
   [SerializeField] float radius;

    private void Start() {
        MassSpawn();
    }
   private void MassSpawn()
   {
    for (int i = 0; i < amount; i++)
    {
        Vector3 offset = Random.insideUnitCircle * radius;
        Instantiate(propPrefab, origin.position + new Vector3(offset.x, offset.z, offset.y), Random.rotation);        
    }
   }
}
