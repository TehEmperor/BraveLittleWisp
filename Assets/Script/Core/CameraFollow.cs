using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float rotationPower = 3f;
    [SerializeField] float rotationLerp = 0.5f;
    [SerializeField] Vector2 _look;
    Quaternion nextRotation;


    private void Start() {
        var angles = transform.localEulerAngles;
        angles.z = 0;
        angles.x = 40;
        transform.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
        
    }
    
    void LateUpdate()
    {

        _look = ComputeLook(Input.mousePosition);
        transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);
        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;
        print(angle);

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
        
    }



    private Vector2 ComputeLook(Vector2 mPos)
    {
        float refX = Screen.width /4;
        float refY = Screen.height /5;
        float retX = 0;
        float retY = 0;
        if(refX >= mPos.x || mPos.x >= Screen.width - refX)
        {
           retX = Mathf.Clamp(mPos.x - refX, -1, 1);
        }
        // if (refY >= mPos.y || mPos.y >= Screen.height - refY)
        // {
        //     retY = Mathf.Clamp(mPos.y - refY, -1, 1);
        // }
        
        return new Vector2(retX, retY); 
    }

    //Casts ray between player and position where camera normally should be. if ray hits something then camera interpolates between
    //its current position and central point of the distance between ray hit and player. Else it interpolates between its current position and position it should be.

    // private void SmoothCameraPosition()
    // {
    //     RaycastHit hit;
    //     Vector3 smoothedPos;
    //     smoothedPos = Vector3.Lerp(transform.position, cameraMount.position, Time.deltaTime * cameraSpeed);
    //     int layerMask = 140;
    //     layerMask = ~layerMask;
    //     if (Physics.Linecast(myPlayer.position, cameraMount.position, out hit, layerMask))
    //     {
    //         Vector3 cameraTargetPosition = hit.point + ((hit.point - myPlayer.position)).normalized;
    //         smoothedPos = Vector3.Lerp(transform.position, cameraTargetPosition, Time.deltaTime * cameraSpeed);
    //     }
    //     transform.position = smoothedPos;
    //     transform.LookAt(myPlayer, cameraMount.transform.up);        
    // }
}
