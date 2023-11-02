using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeCrystal : MonoBehaviour
{
    [SerializeField] float maxCharge = 100f;
    [SerializeField] float currentCharge = 0;
    [SerializeField] float chargeSpeed = 1f;
    [SerializeField] float depletionDistance = 5f;
    [SerializeField] Material emittingMat;
    [SerializeField] Color emissionColor = Color.cyan;
    [SerializeField] GameObject objToMelt = null;
    [SerializeField] Material rayMat;    
    [SerializeField] bool isFull = false;
    [SerializeField] bool isRotating = true;
    [SerializeField] float rotSpeed = 1f;

    public event Action onFullCharge;
    public event Action onFullDepletion;
    private void Start()
    {
        StartCoroutine(FloatRoutine());
        if(emittingMat == null) {emittingMat = GetComponent<Renderer>().material;}
        if(isFull)
        {
            currentCharge = maxCharge;
        }
    }
  
    public void SetMeltObject(GameObject obj)
    {
        objToMelt = obj;
    }

    public bool CheckCharge()
    {
        return isFull;
    }

    public bool CheckDepletion()
    {
        return currentCharge == 0;
    }

    public void Deplete()
    {
        currentCharge = 0;
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(Vector3.Distance(other.gameObject.transform.position, transform.position) <= other.gameObject.GetComponent<PlayerController>().GetLightRange())
            {
            GetCharged(other);
            }
        }
        if(other.gameObject.CompareTag("Enemie"))
        {
            if(Vector3.Distance(other.gameObject.transform.position, transform.position) < depletionDistance)
            {
            GetDepleted();
            }
            return;
        }
    }

    private void GetCharged(Collider other)
    {
        if(isFull) return;
        float value = chargeSpeed * Time.deltaTime;
        other.gameObject.GetComponent<Health>().GetDamaged(value);
        currentCharge = Mathf.Clamp(currentCharge + value, 0, maxCharge);
    }

    private void GetDepleted()
    {
        float value = chargeSpeed * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge - value, 0, maxCharge);
    }

    private void Update()
    {
        SetEmissionIntencity();
        ChargeTrigger();
        {
            if(isFull && !gameObject.CompareTag(Tag.FlASH))
            {
                gameObject.tag = Tag.FlASH;
            }
            else if(gameObject.CompareTag(Tag.FlASH) && currentCharge == 0)
            {
                gameObject.tag = "Untagged";
                onFullDepletion?.Invoke();
            }
        }



    }

    private void ChargeTrigger()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        if (!isFull)
        {
            if (currentCharge < maxCharge) return;
            isFull = true;
            onFullCharge?.Invoke();
            if ( line == null)
            {
                CreateLine(line);
                return;
            }
            line.enabled = true;

        }
        else if(isFull)
        {
            if (line == null)
            {
                CreateLine(line);
            }
            if (currentCharge == maxCharge) return;
            isFull = false;
           
            GetComponent<LineRenderer>().enabled = false;       
        }
    }

    private void CreateLine(LineRenderer line)
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.material = rayMat;
        line.SetPositions(new Vector3[2] { transform.position, objToMelt.transform.position });
    }

    private void SetEmissionIntencity()
    {
        emittingMat.SetColor("_EmissionColor", emissionColor * (currentCharge/100));
    }

    private IEnumerator FloatRoutine()
    {
        while(isRotating)
        {
            transform.Rotate(new Vector3(0,1,0) * rotSpeed, Space.Self);
            yield return new WaitForFixedUpdate();
        }
    }
}
