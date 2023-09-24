using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] [Range(2, 5)]float frameStepX = 2.5f;
    [SerializeField][Range(2, 5)] float frameStepY = 3f;
    [SerializeField] float rotationPower = 3f;
    [SerializeField] float rotationLerp = 0.5f;
    [SerializeField] float cameraSpeed = 2f;
    [SerializeField] Vector2 _move;
    [SerializeField] Vector2 _look;
    [SerializeField] Transform cameraMount;
    Quaternion nextRotation;
    Transform myPlayer;
    bool isRotating = false;


    private void Start() 
    {
        myPlayer = GameObject.FindWithTag("Player").transform;
        transform.position = cameraMount.transform.position;
        transform.rotation = cameraMount.transform.rotation;
        var angles = transform.localEulerAngles;
        angles.z = 0;
        angles.x = 40;
        transform.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
        
    }    

    // public void CameraStabilize() 
    // {
    //     print("Stabilizing");
    //     var angles = transform.localEulerAngles;
    //     angles.z = 0;
    //     angles.x = 40;
    //     angles.y = 0;
    //     transform.localEulerAngles = angles;
    //     nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
    // }
    
    void LateUpdate()
    {
        RaycastHit hit;
        int layerMask = 140;
        layerMask = ~layerMask;
        if (Physics.Linecast(myPlayer.position, cameraMount.position, out hit, layerMask))
        {
            Vector3 cameraTargetPosition = hit.point + ((hit.point - myPlayer.position)).normalized;
            transform.position = Vector3.Lerp(transform.position, cameraTargetPosition, Time.deltaTime * cameraSpeed);
        }
        _move = ComputeLook(GetComponent<Camera>().WorldToScreenPoint(myPlayer.position));
       //SmoothCameraRotation();
        if(_move.x == 0 && _move.y == 0) return;
        SmoothCameraPosition();
    }

    private void SmoothCameraRotation()
    {
        _look = ComputeLook(Input.mousePosition);
        //if( _look.x == 0) return false; 
        transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);
        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;
        //Clamp the Up/Down rotation
        if (angle > 0 && angle < 20)
        {
            angles.x = 20;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        transform.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
        //return true;
    }

    private Vector2 ComputeLook(Vector2 mPos)
    {
        float refX = Screen.width /frameStepX;
        float refY = Screen.height /frameStepY;
        float retX = 0;
        float retY = 0;
        if(refX >= mPos.x || mPos.x >= Screen.width - refX)
        {
           retX = Mathf.Clamp(mPos.x - refX, -1, 1);
        }
        if (refY >= mPos.y || mPos.y >= Screen.height - refY)
        {
            retY = Mathf.Clamp(mPos.y - refY, -1, 1);
        }
        
        return new Vector2(retX, retY); 
    }

    // Casts ray between player and position where camera normally should be. if ray hits something then camera interpolates between
    // its current position and central point of the distance between ray hit and player. Else it interpolates between its current position and position it should be.

    private void SmoothCameraPosition()
    {
        
        Vector3 smoothedPos;
        Quaternion smoothedRot;
        smoothedPos = Vector3.Lerp(transform.position, cameraMount.position, Time.deltaTime * cameraSpeed);
        Vector3 cameraToPlayerVector = (myPlayer.position - cameraMount.position).normalized;
        smoothedRot = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(cameraToPlayerVector, cameraMount.up),Time.deltaTime * cameraSpeed);
        RaycastHit hit;
        int layerMask = 140;
        layerMask = ~layerMask;
        if (Physics.Linecast(myPlayer.position, cameraMount.position, out hit, layerMask))
        {
            Vector3 cameraTargetPosition = hit.point + ((hit.point - myPlayer.position)).normalized;
            smoothedPos = Vector3.Lerp(transform.position, cameraTargetPosition, Time.deltaTime * cameraSpeed);
        }
        transform.position = smoothedPos;
        transform.rotation = smoothedRot;
        
    }
}
