using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] float fMin = 5f;
    [SerializeField] float fMax = 20f;
    [SerializeField] float offset = 0f;
    [SerializeField] GameObject player;
        
    void FixedUpdate()
    {
        FogIndesify();
    }

    private void FogIndesify()
    {
        float value = player.transform.position.y;
        RenderSettings.fogStartDistance = Mathf.Clamp(value + offset, fMin, fMax);
    }

}
