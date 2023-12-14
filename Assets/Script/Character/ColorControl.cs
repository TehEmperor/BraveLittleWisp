using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
    [SerializeField] GameObject pixie;
    [SerializeField] float colorChangeDuration = 0.3f;
    [SerializeField] Color onDamageColor;
    [SerializeField] Color onHealColor;
    [SerializeField] Color regularColor;
    Material myMaterial;
    Health myHealth;
    Coroutine currentColorCroutine;

    
    private void Start() {
        myHealth = GetComponent<Health>();
        myMaterial = pixie.GetComponent<Renderer>().material;
        myHealth.onDmg += DamageColorChange;
        myHealth.onHeal += HealColorChange;
        myMaterial.SetColor("_GlowColor", regularColor);
        
    }

    private void HealColorChange()
    {
        if(currentColorCroutine!= null) return;
        currentColorCroutine = StartCoroutine(onColorChangeRoutine(onHealColor));
    }

    private void DamageColorChange()
    {
        if (currentColorCroutine != null) return;
        currentColorCroutine = StartCoroutine(onColorChangeRoutine(onDamageColor));
    }
    private IEnumerator onColorChangeRoutine(Color _color)
    {
        myMaterial.SetColor("_GlowColor", _color);
        yield return new WaitForSeconds(colorChangeDuration);
        myMaterial.SetColor("_GlowColor", regularColor);
        currentColorCroutine = null;    
    }
}
