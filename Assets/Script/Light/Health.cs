using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] AudioClip healSound;
    [SerializeField] float playerMaxHealth = 100f;
    [SerializeField] float noHealInterval = 5f;
    //Serialized for debug!
    [SerializeField] float playerCurrentHealth;
    [SerializeField] float bonusHealth = 0f;
    bool isAlive = true;
    bool noHeal = false;
    float timeSinceLastDamage = 0;

    public event Action onDeath;
    public event Action onHeal;
    public event Action onDmg;

    private void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        isAlive = true;
    }


    private void Update() 
    {
        timeSinceLastDamage += Time.deltaTime;
        if(timeSinceLastDamage < noHealInterval)
        {
            if(!noHeal){noHeal = true;}
            return;
        }
        if(noHeal) {noHeal = false;};
        
        
    }


    public void GetDamaged(float dmg)
    {
        if (!isAlive) return;
        onDmg?.Invoke();
        timeSinceLastDamage = 0;
        if(bonusHealth > 0)
        {
            float bhealthAfterDmg = Mathf.Clamp(bonusHealth - dmg, 0f, playerMaxHealth * 2);
            bonusHealth = bhealthAfterDmg;
            return;
        }
        float healthAfterDmg = Mathf.Clamp(playerCurrentHealth - dmg, 0f, playerMaxHealth);
        playerCurrentHealth = healthAfterDmg;
        if (playerCurrentHealth == 0)
        {
            Die();
        }
    
    }

    public void OverchargeHealth(float amount)
    {
        float overchargedHealth = Mathf.Clamp(bonusHealth + amount, 0, playerMaxHealth * 2);
        bonusHealth = overchargedHealth;
    }

    public bool CheckPulse()
    {
        return isAlive;
    }

    public float GetLightIntesityModifier()
    {
        return (playerCurrentHealth + bonusHealth) / playerMaxHealth;
    }

    public float GetOverchargedHealth()
    {
        return bonusHealth;
    }



    private void Die()
    {
        isAlive = false;
        onDeath?.Invoke();
    }

    public void GetHealed(float heal)
    {
        if(noHeal) return;       
        float healthAfterHeal = Mathf.Clamp(playerCurrentHealth + heal, 0f, playerMaxHealth);
        if (healthAfterHeal < playerMaxHealth)
        {
            onHeal?.Invoke();
            GetComponent<AudioSource>().PlayOneShot(healSound);
        }
        playerCurrentHealth = healthAfterHeal;
    }


}
