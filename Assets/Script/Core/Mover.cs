using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    NavMeshAgent myAgent;
    [SerializeField] Animator myAnimator;
    [SerializeField] float speed;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        speed = myAgent.speed;
    }
    private void Update() 
    {
        HandeAnimation();
    }
    public void MoveTo(Vector3 destination, float speedOffset)
    {
        myAgent.speed = speed * speedOffset;
        myAgent.destination = destination;        
               
    }

    private void HandeAnimation()
    {
        if(!myAnimator) return;
        Vector3 velocity = myAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        myAnimator.SetFloat("Speed", speed);

    }
    public void Cancel()
    {
        myAgent.isStopped = true;
    }
}
