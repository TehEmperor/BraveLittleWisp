using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapLightControl : MonoBehaviour
{
    [SerializeField] Volume NightVolume;
    [SerializeField] Volume DayVolume;
    WorldMapKeeper myKeeper;
    private void Awake() 
    {
        myKeeper = FindObjectOfType<WorldMapKeeper>();
        myKeeper.onLightCalculated+=SetLight;
    }

    private void SetLight(float fraction)
    {
        float modifier = Mathf.Clamp(fraction, 0, 1);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientLight = Color.white * modifier;
        RenderSettings.ambientGroundColor = Color.black;
        NightVolume.weight = 1-modifier;
        DayVolume.weight = modifier;

    }
}
