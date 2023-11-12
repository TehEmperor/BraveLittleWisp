using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBEG : MonoBehaviour
{
    [SerializeField] State startState = State.PhaseOne; //serialized for testing
    
    [SerializeField] BossEmber thrownEmberPrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] float throwInterval = 4f;
    [SerializeField] float maxEnemieCount = 2f;
    [SerializeField] float pPosUpdateInterval = 0.3f;
    [SerializeField] float corruptionPlacementInterval = 1f;
    [SerializeField] ChargeCrystal canon;
    [SerializeField] Transform[] phaseTwoMinionSpawns;
    [SerializeField] Minion minionPrefab;
    [SerializeField] Transform crystalMass;
    [SerializeField] float crystalrevialinterval = 0.2f;
     List<Enemie> enemiesPresent = new List<Enemie>();
    float timeSinceLastThrow = 0f;
    float timeSinceLastPPUpdate = 0f;
    float timeSinceLastCorruption = 0f;
    float maxCorruptionPlacementInterval = 0f;
    GameObject target;
    List<Vector3> followPathPhTwo = new List<Vector3>();
    enum State 
    {
        PhaseOne,
        Interphase,
        PhaseTwo,
        PlayerLost,
        PlayerWon
    }

    State state;
    
    private void Start() 
    {
        maxCorruptionPlacementInterval = corruptionPlacementInterval;
        target = GameObject.FindWithTag("Player");      
        state = startState;
        canon.onFullCharge+=GetRekt;
        StartCoroutine(CrystalsGrowth(true));
    }


    private void Update()
    {

        switch (state)
        {
            default:
            case State.PhaseOne:
            if(MeansToAnEnd()) return;
            ThrowEmber(); 
            UpdateTimers();
                break;
            case State.Interphase:
                PreparePhaseTwo();
                
                break;
            case State.PhaseTwo:
                ModifyCorruptionIntencity();
                PlayerFollowListUpdate();
                CorruptPath();
                UpdateTimers();            
                break;
            case State.PlayerLost:
            break;
            case State.PlayerWon:
            break;

        }
        
    }

    private void UpdateTimers()
    {
        timeSinceLastThrow += Time.deltaTime;
        if(state == State.PhaseOne) return;
        timeSinceLastPPUpdate += Time.deltaTime;
        timeSinceLastCorruption+= Time.deltaTime;
    }

    private void ThrowEmber()
    {
        if(timeSinceLastThrow < throwInterval) return;
        BossEmber ember = Instantiate(thrownEmberPrefab, throwPoint.position, Quaternion.identity);
        ember.SetTarget(target.transform.position);
        ember.onEnemieSpawn+=UpdateSpawnedEnemiesList;
        timeSinceLastThrow = 0f;
    }
    private void UpdateSpawnedEnemiesList(Enemie enemie)
    {
     enemiesPresent.Add(enemie);   
    }

    private bool MeansToAnEnd()
    {
        if(enemiesPresent.Count != maxEnemieCount) return false;
        else
        {
            StartCoroutine(GoForthMyMinions());
            state = State.PlayerLost;
            return true;
        } 
    }

    private IEnumerator GoForthMyMinions()
    {
        while (target !=null)
        {        
        foreach(Enemie enemie in enemiesPresent)
        {
            enemie.RoamingUpdate(target.transform.position, 5);
        }
        yield return new WaitForSeconds(0.3f);
        }
    }

    private void UndoMinions()
    {
        foreach (Enemie enemie in enemiesPresent)
        {
            Vector3 position = enemie.transform.position;
            enemie.BurnAndDie();
            Instantiate(minionPrefab, position, Quaternion.identity);
        }
    }

    private void GetRekt()
    {
        StopAllCoroutines();        
        if(state == State.PhaseOne)
        {
        state = State.Interphase;
        return;}
        else {state = State.PlayerWon;};
    }

    private void PreparePhaseTwo()
    {
        UndoMinions();
        canon.Deplete();
        //also here should be played the animation for boss preparing for his next phase and in the end of animation the phase sshould be se to phasetwo;
        //meanwhile
        state = State.PhaseTwo;

    }

    private void PlayerFollowListUpdate()
    {
        if(timeSinceLastPPUpdate >= pPosUpdateInterval)
        {
        followPathPhTwo.Add(target.transform.position);
        timeSinceLastPPUpdate = 0;
        }
    }

    private void CorruptPath()
    {
        if(timeSinceLastCorruption >= corruptionPlacementInterval)
        {
            PlaceCorruption();
            timeSinceLastCorruption = 0;
        }
    }

    private void PlaceCorruption()
    {
        GameObject corruption = CorruptionPool.SharedInstance.GetPooledCorruption();
        if (corruption != null)
        {
            Vector3 position = GetNextCorruptionPosition();
            corruption.transform.position = position;
            followPathPhTwo.Remove(position);
            corruption.SetActive(true);
        }
    }

    private Vector3 GetNextCorruptionPosition()
    {
        if(followPathPhTwo.Count > 0)
        {
        return followPathPhTwo[0];        
        }
        else return target.transform.position;

    }

    IEnumerator CrystalsGrowth(bool onoff)
    {
        for (int i = 0; i < crystalMass.childCount; i++)
        {
            crystalMass.GetChild(i).gameObject.SetActive(onoff);
            yield return new WaitForSeconds(crystalrevialinterval);            
        }

    }

    private void ModifyCorruptionIntencity()
    {
        float mod = target.GetComponent<PlayerController>().GetLightAsFracture();
        Debug.Log(mod);        
        corruptionPlacementInterval = Mathf.Clamp(maxCorruptionPlacementInterval - mod, 0.1f, maxCorruptionPlacementInterval);
    }

}
