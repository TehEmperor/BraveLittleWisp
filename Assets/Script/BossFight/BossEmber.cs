using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEmber : MonoBehaviour
{
    [SerializeField] Enemie enemiePrefab;
    [SerializeField] float speed = 5f;
    [SerializeField] Vector3 offset = new Vector3(0,-0.5f, 0);
    [SerializeField] float fadeDuration = 5f;
    [SerializeField] Color emissionColor = Color.red;
    [SerializeField] float maxIntencity = 1f;
   
    private float minIntencity = 0f;
    private Vector3 target;

    public event Action<Enemie> onEnemieSpawn;
    public event Action onDestroy;

    private void Start()
    {
        // GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor * maxIntencity);
        // StartCoroutine(FadeAway());
        transform.LookAt(target+offset);  
    }

    private void Update() 
    {
      FlyAtTarget(target + offset, speed);   
    }


    IEnumerator FadeAway()
    {
        float time = 0;
        while (time < fadeDuration)
        {
            float value = Mathf.Lerp(maxIntencity, minIntencity, time / fadeDuration);
            GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor * value);
            time += Time.deltaTime;
            yield return null;
        }        
    }
    private void FlyAtTarget(Vector3 target, float speedMod)
    {
        Vector3 aimLocation = target;        
        transform.Translate(Vector3.forward * Time.deltaTime * speedMod);  
    }

    private void OnCollisionEnter(Collision other)
    {
       if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground"))
        {
            SpawnEnemie();
        }
        else DestroySelf(); 

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flameable")) return;
        DestroySelf();

    }

    private void DestroySelf()
    {
        onDestroy?.Invoke();
        Destroy(gameObject);
    }

    private void SpawnEnemie()
    {
       Enemie enemie = Instantiate(enemiePrefab,transform.position,Quaternion.identity);
       onEnemieSpawn?.Invoke(enemie);
       Destroy(gameObject);
    }

    public void SetTarget(Vector3 _target)
    {
        target = _target;
    }
    
}
