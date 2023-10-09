using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterFX : MonoBehaviour
{
    [SerializeField] MeshRenderer mySurface;
    private void OnTriggerEnter(Collider other) 
    {
        if(!other.gameObject.CompareTag("Player")) return;
        RenderSettings.fog = true;
        mySurface.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        RenderSettings.fog = false;
        mySurface.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
}
