using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : MonoBehaviour
{    
    [SerializeField] float existanceTime = 2f;
    [SerializeField] float damage = 1f;
    private void OnEnable()
    {
        StartCoroutine(WaitAndTurnOff());
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().GetDamaged(damage*Time.deltaTime);
        }
        
    }

    IEnumerator WaitAndTurnOff()
    {
        yield return new WaitForSeconds(existanceTime);
        gameObject.SetActive(false);
    }
}
