using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimney : MonoBehaviour
{
    [SerializeField] float emberDropInterval = 0.5f;
    [SerializeField] bool isDroping = true;

    public event Action<Vector3> onEmberPlaced;
    private void Start() {
        StartCoroutine(LeaveEmbers());
    }
    IEnumerator LeaveEmbers()
    {
        while(isDroping)
        {
            yield return new WaitForSeconds(emberDropInterval);
            DropEmber();
        }
    }

    private void DropEmber()
    {
    GameObject ember = EmberPool.SharedInstance.GetPooledEmber(); 
    if (ember != null) 
    {
    ember.transform.position = transform.position;
    onEmberPlaced?.Invoke(transform.position);
    ember.SetActive(true);
    }
    }
}
