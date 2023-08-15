using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterFX : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(!other.gameObject.CompareTag("Player")) return;
        RenderSettings.fog = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        RenderSettings.fog = false;
    }
}
