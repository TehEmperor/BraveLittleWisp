using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ember : MonoBehaviour
{
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] Color emissionColor = Color.red;
    [SerializeField] float maxIntencity = 1f;
    private float minIntencity = 0f;


    private void OnEnable() 
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor * maxIntencity);
        StartCoroutine(FadeAway());
    }

    

    IEnumerator FadeAway()
    {
        float time = 0;
        while(time<fadeDuration)
        {
            float value = Mathf.Lerp(maxIntencity, minIntencity, time/fadeDuration);
            GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor * value);
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
