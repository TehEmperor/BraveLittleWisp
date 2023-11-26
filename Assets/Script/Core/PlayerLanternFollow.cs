using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLanternFollow : MonoBehaviour
{
    public Transform followPoint;
    [SerializeField] float lerpSpeed = 2f;
    public Throw myThrow = null;

   private void Update() 
   {
    LightSourceFollow();
    if(myThrow)
    {
    Show(myThrow.CoolDown());    
    }
   }

    void Show(bool show)
    {
        GetComponent<MeshRenderer>().enabled = !show;
    }

    private void LightSourceFollow()
    {
        Vector3 smoothedPos;
        smoothedPos = Vector3.Lerp(transform.position, followPoint.position, Time.deltaTime * lerpSpeed);
        transform.position = smoothedPos;
    }
}
