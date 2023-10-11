using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] Level myLevel = null;
    [SerializeField] bool isSecret = false;
    [SerializeField] Fader fader;
    public GameObject fireColumnPrefab;
    public float fadeOutDuration = 1f;
    public float fadeInDuration = 1f;
    public int sceneToLoad = -1;
    public ParticleSystem[] fires;
    private int soulsPresent = 0;
    int soulsCollected;
    bool isActivated = false;
    GameObject player;
    public event Action onAllSoulsCollected;
    public static event Action<Transform> onPlayerWithinRange;   
      
    private void Start()
    {
        soulsPresent = SoulsRquiredToActivateCount();
        fires =  SpawnColumns(fireColumnPrefab,soulsPresent);
        HushFires(isActivated);                    
    }

    private int SoulsRquiredToActivateCount()
    {
        if(myLevel) {return myLevel.GetSoulsToActivate();}      
        int var = FindObjectsOfType<Minion>().Length;
        if(var == 0) return 1;
        return var;

    }

    public void SetLevel(Level level)
    {
        myLevel = level;
    }


    public void HushFires(bool isActive)
    {
        foreach(ParticleSystem fire in fires)
        {
            if(isActive == false)
            {
                fire.Stop();
                continue;
            }
            else if(isActive)
            {
                fire.Play();
            }
            
        }
    }

    public void LitColumn()
    {
        foreach(ParticleSystem fire in fires)
        {
            if(fire.isStopped)
            {
                fire.Play();
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag(Tag.COLLECTABLE))
        {   
            LitColumn();
            other.gameObject.GetComponent<Minion>().StartTheAscention(); // this is where we should activate any possible animations for when we bring souls.
            soulsCollected++;
            if(soulsCollected >= soulsPresent)
            {
                isActivated = true;           
                onAllSoulsCollected?.Invoke();                
            }            
        }

        else if (other.gameObject.CompareTag(Tag.PLAYER))
        {
            onPlayerWithinRange?.Invoke(this.transform);
            player = other.gameObject;
            if(!isActivated) return;
            soulsPresent = 0;
            StartCoroutine(LoadNextLevel());
        }
    }

    //this fades screen into white, then loads next level, then fades back to normal;
    public IEnumerator LoadNextLevel()
    {        
        player.GetComponent<PlayerController>().enabled = false;
        yield return fader.FadeOut(fadeOutDuration);       
        GameObject.DontDestroyOnLoad(this);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        UpateWorldKeeper();
        yield return fader.FadeIn(fadeInDuration);
        Destroy(gameObject);
    }

    private void UpateWorldKeeper()
    {
        myLevel.Finish();
        myLevel.DiscoverNextLevels(isSecret);
        WorldMapKeeper keeper = FindObjectOfType<WorldMapKeeper>();
        keeper.LevelChange(myLevel);
    }

    public void ActivateSecret(bool onoff)
    {
        isSecret = onoff;
    }   
    

    public ParticleSystem[] SpawnColumns(GameObject columnPrefab, int amount)
    {
        float radius = GetComponent<SphereCollider>().radius;
        ParticleSystem[] columnsToLit = new ParticleSystem[amount];
        for(int currentColumn = 0; currentColumn<amount; currentColumn++)
        {
            float crcmfProgress = (float)currentColumn/amount;
            float currentRadian = crcmfProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float zScaled = Mathf.Sin(currentRadian);
            float x = xScaled * radius;
            float z = zScaled * radius;
            Vector3 posAdjustment = new Vector3(x, 0, z);
            Vector3 cSpawnPos = transform.position + posAdjustment;
            GameObject fireColumn = Instantiate(columnPrefab, cSpawnPos, Quaternion.identity, this.transform);
            fireColumn.GetComponent<ColumnRotation>().SetRotation(transform.position);
            columnsToLit[currentColumn] = fireColumn.GetComponentInChildren<ParticleSystem>();
        }
        return columnsToLit;
        
    }
}
