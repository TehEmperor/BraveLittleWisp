using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flash : MonoBehaviour
{
    [SerializeField] bool isHoming = false;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] float speed = 1f;
    [SerializeField] Transform lightOutline;
    bool onGround;
    Vector3 target;
    public float lightRadius;


    private void Start() 
    {
        onGround = false;
        if (!isHoming)
        {
            transform.LookAt(target);
        }
        Destroy(gameObject, lifeTime);
        GetComponent<Light>().range = lightRadius;
        GetComponent<SphereCollider>().radius = lightRadius;
        lightOutline.localScale = new Vector3(2, 2, 2) * lightRadius;
        
    }

    void Update()
    {
        if (onGround) return;       
        FlyAtTarget(target, speed, isHoming);
    }

    private void FlyAtTarget(Vector3 target, float speedMod, bool homing)
    {        
        Vector3 aimLocation = target;
        if (homing == false)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speedMod);
            return;
        }

        transform.LookAt(aimLocation);

        transform.Translate(Vector3.forward * Time.deltaTime * speedMod);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        onGround = true;
       // print(other.gameObject.name);
        
    }

    public void SetTarget(Vector3 _target)
    {
        target = _target;
    }
    
    
}
