using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class CameraFollow : MonoBehaviour
{   
    [SerializeField] float rotationPower = 0.3f;
    [SerializeField] float rotationLerp = 0.5f;
    [SerializeField] Vector2 _move;
    [SerializeField] Vector2 _look;
    [SerializeField] Transform cameraFollow;
    Quaternion nextRotation;
    
    public void OnMove(InputValue value)
    {

    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
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
        SmoothCameraRotation();
       
    }

    private void SmoothCameraRotation()
    {
        cameraFollow.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
        //cameraFollow.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

        var angles = cameraFollow.transform.localEulerAngles;
        angles.z = 0;
        var angle = cameraFollow.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 20;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        cameraFollow.transform.localEulerAngles = angles;

        nextRotation = Quaternion.Lerp(cameraFollow.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
    }
}
