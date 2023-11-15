using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    [SerializeField] float damageInterval = 1f, hitPoint = 5f; // 20f;
    [SerializeField] AudioClip discoverPlayerClip;
    [SerializeField] AudioClip suckLightClip;
    [SerializeField] GameObject currentDistraction = null;
    [SerializeField] Minion minionPrefab;
    Vector3 originPosition;
    Vector3 roamToPosition = Vector3.zero;
    float speedOffset = 1f;
    [SerializeField] Health chaseTarget = null;
    Mover myMover;
    Coroutine suckLightRoutine;
    Coroutine deathRoutine = null;

    [SerializeField] float minimumTriggerDistance = 20; //Check if the flash is within its sense radius. 

    [SerializeField] bool isChasing;
    bool isOperational = true;
    public static event Action<GameObject> OnEnemyDetection;
    public static event Action OnEnemyDetectionBool;
    public static event Action<float> IntensityControl;


    private void Awake()
    {
        
        myMover = GetComponent<Mover>();

    }
    private void Start()
    {
        originPosition = transform.position;
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tag.PLAYER))
        {
            chaseTarget = other.gameObject.GetComponent<Health>();
            OnEnemyDetection?.Invoke(this.gameObject);
        }
        else if(other.gameObject.CompareTag(Tag.FlASH))
        {
            print(other.gameObject.name);
            currentDistraction = other.gameObject;
        }

    }

    private void Update()
    {
        if(!isOperational) return;
        if(GetDistracted()) return;
        if (Chase()) return;
        Roam();
    }

    IEnumerator SuckLight()
    {
        while (isChasing)
        {
            yield return new WaitForSeconds(damageInterval);
            if (chaseTarget.CheckPulse())
            {
                GetComponent<AudioSource>().PlayOneShot(suckLightClip);
                chaseTarget.GetDamaged(hitPoint);
            }
        }
    }

    private bool Chase()
    {
        if (!chaseTarget) return false;
        float chaseDistance = chaseTarget.GetComponent<PlayerController>().GetLightRange() + 1;
        if (Vector3.Distance(transform.position, chaseTarget.transform.position) <= chaseDistance)
        {
            if (!isChasing) GetComponent<AudioSource>().PlayOneShot(discoverPlayerClip); //Music is not playing for player detection for some reason; 
            isChasing = true;
            if (suckLightRoutine == null) suckLightRoutine = StartCoroutine(SuckLight());
            myMover.MoveTo(chaseTarget.transform.position, speedOffset);
            return true;
        }
        isChasing = false;        
        return false;
    }

    public void BurnAndDie()
    {
        if(deathRoutine != null) return;
        StopAllCoroutines();
        deathRoutine = StartCoroutine(SlowBurn());
    }

    IEnumerator SlowBurn()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
        isOperational = false;
        GetComponent<Animator>().Play("CleansedByFlame");       
    }

    void MinionSpawn()
    {
        Instantiate(minionPrefab, transform.position, Quaternion.identity);
    }

    void GetDestroyed()
    {        
        Destroy(gameObject);
    }

    

    private void Roam()
    {
        if(isChasing){isChasing = false;}
        if(suckLightRoutine!=null) {suckLightRoutine= null;}
        if(roamToPosition == Vector3.zero)
        {
            myMover.MoveTo(originPosition, 1);            
            return;
        }
        myMover.MoveTo(roamToPosition, speedOffset);
        if(Vector3.Distance(transform.position, roamToPosition) <= 1f)
        {
            roamToPosition = Vector3.zero;
        }
    }

    public void RoamingUpdate(Vector3 roamTo, float speedFracture)
    {        
        roamToPosition = roamTo;
        speedOffset = speedFracture;
    }
        

    bool GetDistracted()
    {
        if(currentDistraction == null) return false;
        if(!currentDistraction.CompareTag(Tag.FlASH)) return false;
        if(isChasing) isChasing = false;
        suckLightRoutine = null;
        myMover.MoveTo(currentDistraction.transform.position, 1);
        return true;
    }

}
