
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] float lightMaxRange = 18f, lightMinIntensity = 0f, rangeIntencityStep = 200f;
    [SerializeField] [Range(0,1)] float effectiveRangeMod = 0.8f;
    [SerializeField] PlayerController myPlayer;
    [SerializeField] float lerpSpeed = 20f;    
    [SerializeField] float lightYRangeMin = 0.3f, lightYrangeMax = 6.8f; 
    float currentMnY;
    Health myHealth;
    private float steps = 1;  //Lerp Steps; 

    Light myLight = null;
    private void OnEnable()
    {
        myPlayer.onChangeBrightness += LightControl;
    }

    private void OnDisable()
    {
        myPlayer.onChangeBrightness -= LightControl;
    }


    private void Awake()
    {
        myLight = GetComponent<Light>();
        myHealth = myPlayer.GetComponent<Health>();

    }
    private void Start()
    {
        myLight.range = lightMaxRange;
        currentMnY = transform.localPosition.y;
        lightYrangeMax = lightMaxRange * 0.5f;
        CalculateLightHeight();
       
    }

    public float GetLightRange()
    {
        float effectiveRange = myLight.range * effectiveRangeMod;
        return effectiveRange;
    }

    public float GetLightAsPercentage()
    {
        return 1/(lightMaxRange/myLight.range);
    }

    void Update()
    {
        LightIntesityEffects();
       // LighSourceFollow();
    }

    private void LighSourceFollow()
    {
       // Vector3 smoothedPos;
        //smoothedPos = Vector3.Lerp(transform.position, lightPlace.position, Time.deltaTime * lerpSpeed);
        //transform.position = smoothedPos;
    }

    public void LightControl(float amount)
    {
        if (myLight.range + amount > lightMaxRange) return;
        if (myLight.range + amount < 2) return; //We need to see our surrounding hence keeping some light (Previous value was 0)
        myLight.range += amount;
        CalculateLightHeight();
    }

    private void CalculateLightHeight()
    {
                
        Vector3 lightHeight = new Vector3(transform.localPosition.x, myLight.range/2, transform.localPosition.z);
        transform.localPosition = lightHeight;
    }

    void LightIntesityEffects()
    {
        var amount = rangeIntencityStep * myLight.range * (myHealth.GetLightIntesityModifier());        
        if (amount < lightMinIntensity) return;
        myLight.intensity = Mathf.Lerp(myLight.intensity, amount, steps * Time.deltaTime);
    }
}
