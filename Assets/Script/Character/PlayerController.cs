using System.ComponentModel;
using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] LightSource myLightSource;
    [SerializeField] float lightRadiusDetectionOffset = 2f;
    [SerializeField] AudioClip onDeathSound;
    [SerializeField] float maxNMProjectionDistance = 1f; //for checking if the click is near navMesh
    [SerializeField] float maxNavPathLength = 40f; //for checking if the path to a desired point is not too long so the player is not moving if the click is too far away

    [SerializeField] UnityEvent onMouseClick;
    [SerializeField] float speedMod = 1f;
    public event Action<float> onChangeBrightness; //We could make it static
    public static event Action<Vector3> OnEnemyDetection;
    public static event Action onLastRights;
    private Health myHealth;
    private Mover myMover;
    private float yVelocity; //The y velocity of the player
    private Vector3 moveDirection;
    private CharacterController characterController;

    private void Awake()
    {
        myHealth = GetComponent<Health>();
        myHealth.onDeath += LastRights;
        myMover = GetComponent<Mover>();

    }
    private void OnDisable()
    {
        myHealth.onDeath -= LastRights;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onMouseClick.Invoke();
        }
        if (myHealth.CheckPulse() == false) return;

        //Optional mouse Scroll Wheel; 
        if (Input.GetAxis(Axis.SCROLL_WHEEL) < 0f)
        {
            onChangeBrightness?.Invoke(-1);
        }
        if (Input.GetAxis(Axis.SCROLL_WHEEL) > 0f)
        {
            onChangeBrightness?.Invoke(1);
        } //optional mouse scroll wheel 


        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            onChangeBrightness?.Invoke(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            onChangeBrightness?.Invoke(1);
        }



        if (InteractWithMovement()) return;

    }

    public float GetLightRange()
    {
        return myLightSource.GetLightRange(); // - lightRadiusDetectionOffset;
        //we can use offset instead of hard code; 
    }

    public float GetLightAsFracture()
    {
        return myLightSource.GetLightAsPercentage();
    }

    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    public bool RaycastNavMesh(out Vector3 target)
    {
        target = Vector3.zero;
        Ray ray = GetMouseRay();
        RaycastHit hit;
        int layerMask = 156;
        layerMask = ~layerMask;
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) return false;
        NavMeshHit nHit;
        if (!NavMesh.SamplePosition(hit.point, out nHit, maxNMProjectionDistance, NavMesh.AllAreas)) return false;
        target = nHit.position;
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
        if (!hasPath) return false;
        if (path.status != NavMeshPathStatus.PathComplete) return false;
        if (GetPathLength(path) > maxNavPathLength) return false;
        return true;

    }

    private float GetPathLength(NavMeshPath path)
    {
        float pathLength = 0;
        if (path.corners.Length < 2) return pathLength;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            pathLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }
        return pathLength;
    }

    private bool InteractWithMovement()
    {
        Vector3 target;
        bool hasHit = RaycastNavMesh(out target);
        if (hasHit)
        {
            if (Input.GetMouseButton(0))
            {
                myMover.MoveTo(target, speedMod);
            }

            return true;
        }

        return false;
    }

    private void LastRights()
    {
        GetComponent<AudioSource>().PlayOneShot(onDeathSound);
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Death");
        StartCoroutine(WaitTillLastRightsFinished(1.8f)); //hardcoded. Should make logic better but for now it's ok
    }

    IEnumerator WaitTillLastRightsFinished(float time)
    {
        yield return new WaitForSeconds(time);
        onLastRights?.Invoke();
    }

    public void GetSlowed(float time)
    {
        StartCoroutine(SlowEffect(time));
    }

    IEnumerator SlowEffect(float time)
    {
        speedMod = 0f;
        yield return new WaitForSeconds(time);
        speedMod = 1f;
    }

   
}
