using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInputReciever : MonoBehaviour
{
    [SerializeField] float rotationPower = 0.3f;
    [SerializeField] float rotationLerp = 0.5f;
    [SerializeField] float speed = 1f;
    [SerializeField] Vector2 _move;
    [SerializeField] Vector2 _look;
    [SerializeField] Transform cameraFollow;
    float rmbValue = 0;
    
    public Vector3 nextPosition;
    NavMeshAgent myAgent;
    Quaternion nextRotation;

    private void OnEnable()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    public void OnMouse(InputValue value)
    {
        rmbValue = value.Get<float>();
        
    }

    private void Start()
    {
        var angles = cameraFollow.transform.localEulerAngles;
        angles.z = 0;
        angles.x = 40;
        cameraFollow.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(cameraFollow.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
    }

    void LateUpdate()
    {
        cameraFollow.transform.position = transform.position;
        CameraRotation();
    }

    private void CameraRotation()
    {
        
        

        if (_move.x == 0 && _move.y == 0)
        {
            nextPosition = transform.position;
            
        }
        float moveSpeed = speed/100;
        Vector3 position = (transform.forward * _move.y * moveSpeed) + (transform.right * _move.x * moveSpeed);
        nextPosition = transform.position + position;

        if(rmbValue > 0)
        {
            cameraFollow.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
            cameraFollow.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

            var angles = cameraFollow.transform.localEulerAngles;
            angles.z = 0;
            var angle = cameraFollow.transform.localEulerAngles.x;

            //Clamp the Up/Down rotation
            if (angle > 0 && angle < 20)
            {
                angles.x = 20;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }
            cameraFollow.transform.localEulerAngles = angles;
            nextRotation = Quaternion.Lerp(cameraFollow.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
            
        //transform.rotation = Quaternion.Euler(0, cameraFollow.transform.rotation.eulerAngles.y, 0);
        }
        //cameraFollow.transform.localEulerAngles = new Vector3(angles.x, 0, 0);        
    }
}

