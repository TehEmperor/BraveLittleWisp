using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLanternFollow : MonoBehaviour
{
    [SerializeField] Transform followPoint;
    [SerializeField] float lerpSpeed = 2f;
   private void Update() {
    LightSourceFollow();
    
   }

    private void LightSourceFollow()
    {
        Vector3 smoothedPos;
        smoothedPos = Vector3.Lerp(transform.position, followPoint.position, Time.deltaTime * lerpSpeed);
        transform.position = smoothedPos;
    }
}
