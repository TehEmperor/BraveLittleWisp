using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterFX : MonoBehaviour
{
    [SerializeField] MeshRenderer mySurface;
    [SerializeField] Volume myVolume;
    private void OnTriggerEnter(Collider other) 
    {
        if(!other.gameObject.CompareTag("Player")) return;
        RenderSettings.fog = true;
        myVolume.weight = 1;        
        if(mySurface){ mySurface.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;}
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        RenderSettings.fog = false;
        myVolume.weight = 0;
        if (mySurface) { mySurface.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On; }
    }
}
