using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingTide : MonoBehaviour
{
    [SerializeField] float maxY =  20f;
    [SerializeField] float secondsToHighTide = 120f;
    [SerializeField] float secondsToLowTide = 240f;
    float time = 0;
    Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(RiseTheTide());
    }


    IEnumerator RiseTheTide()
    {
        time = 0;
        while(time < secondsToHighTide)
        {
            transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0,maxY,0), time / secondsToHighTide);
            time += Time.deltaTime;
            yield return null;            
        }
        
        StartCoroutine(LowerTheTide());
    }

    IEnumerator LowerTheTide()
    {
        time = 0;
        while (time < secondsToLowTide)
        {
            transform.position = Vector3.Lerp(startPosition + new Vector3(0, maxY, 0), startPosition, time / secondsToLowTide);
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(RiseTheTide());
    }
}
