using System.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [SerializeField] ParticleSystem[] mySoul;
    private NavMeshAgent agent;
    Coroutine healingCoroutine;
    Mover myMover = null;
    [SerializeField] float followRange = 2f;
    private float stoppingDistance = 1;
    [SerializeField] private float fleeDistance = 5;

    [SerializeField] float healInterval = 1, healPoint = 5;

    private float lightSourceIntensityVariable, maxLightSourceIntensity; //it will be change based on the player health and light source intensity

    [SerializeField] bool isFollowing;
    [SerializeField] AudioClip greetSound;
    [SerializeField] AudioClip ascendSound;
    
    bool isAscending;
    private PlayerController player;
    GameObject enemy;

    private void Awake()
    {
        Enemie.OnEnemyDetection += EnemyPosition;
        myMover = GetComponent<Mover>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tag.PLAYER).GetComponent<PlayerController>();
        Portal.SoulCount();
    }
    private void Start()
    {
        StopParticle();
        isAscending = false;
        
    }

    private void OnDisable()
    {
        Enemie.OnEnemyDetection -= EnemyPosition;
    }

    void Update()
    {
        if(isAscending) return;
        if(FleeEnemy()) return;
        FollowPlayer();
        
    }

    bool BoolCheck(Vector3 pos1, Vector3 pos2)
    {
        var dist = Vector3.Distance(pos1, pos2);
        if (dist <= player.GetLightRange()) return true;
        return false;
    }

    void FollowPlayer()
    {
        if (BoolCheck(gameObject.transform.position, player.transform.position))
        {
            if(!isFollowing)
            {
                GetComponent<AudioSource>().PlayOneShot(greetSound);
                isFollowing = true;
            }            
            StartParticle();
            if (healingCoroutine == null)
            {
                healingCoroutine = StartCoroutine(Healing());
            }

            NavMeshPath path = new NavMeshPath();
            Vector3 playerToMe = (transform.position - player.transform.position).normalized;
            Vector3 followPlayerPosition = player.transform.position + playerToMe * followRange;
            bool hasPath = NavMesh.CalculatePath(transform.position, followPlayerPosition, NavMesh.AllAreas, path);
            if (!hasPath)
            {
                myMover.MoveTo(player.transform.position, 1);
                return;
            }
            myMover.MoveTo(player.transform.position + (playerToMe * followRange), 1);
           
        }
        else
        {
            StopParticle();
            healingCoroutine = null;
            isFollowing = false;
        }
        

    }


    private bool FleeEnemy()
    {
        if(enemy)
        {        
         if (Vector3.Distance(enemy.transform.position, transform.position) <= fleeDistance)
            {
            isFollowing = false;
            StopParticle();
            var fleeVector = enemy.transform.position - this.transform.position;
            var moveVector = this.transform.position - fleeVector;
            myMover.MoveTo(moveVector, 1.3f);
            healingCoroutine = null;
            return true;
            }
            return false;
        }
        return false;

    }

    IEnumerator Healing()
    {
        while (isFollowing)
        {
            yield return new WaitForSeconds(healInterval);
            if(player.GetComponent<Health>().CheckPulse())
            {
               player.GetComponent<Health>().GetHealed(healPoint);
            }
            
        }

    }

    void EnemyPosition(GameObject _enemy)
    {
        enemy = _enemy;
    }


    void StartParticle()
    {
        if (mySoul[0].isStopped)
        {
            foreach (ParticleSystem particle in mySoul)
            {
                particle.Play();
            }
        }
    }

    void StopParticle()
    {
        if (mySoul[0].isPlaying)
        {
            foreach (ParticleSystem particle in mySoul)
            {
                particle.Stop();
            }
        }
    }
    public void StartTheAscention()
    {
        GetComponent<BoxCollider>().enabled = false;
        myMover.Cancel();
        GetComponent<AudioSource>().PlayOneShot(ascendSound);
        isAscending = true;
        GetComponent<Animator>().SetTrigger("Ascend");        
    }

    //animation event
    void Ascended()
    {
        Destroy(gameObject);
    }
}



