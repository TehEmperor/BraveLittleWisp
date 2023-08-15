using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform cameraMount;
    [SerializeField] float cameraSpeed = 20f;
    Transform myPlayer;
   

   private void Awake() 
   {
    myPlayer = GameObject.FindWithTag(Tag.PLAYER).transform;
   }

    
    void LateUpdate()
    {
        SmoothCameraPosition();
    }

    //Casts ray between player and position where camera normally should be. if ray hits something then camera interpolates between
    //its current position and central point of the distance between ray hit and player. Else it interpolates between its current position and position it should be.

    private void SmoothCameraPosition()
    {
        RaycastHit hit;
        Vector3 smoothedPos;
        smoothedPos = Vector3.Lerp(transform.position, cameraMount.position, Time.deltaTime * cameraSpeed);
        int layerMask = 140;
        layerMask = ~layerMask;
        if (Physics.Linecast(myPlayer.position, cameraMount.position, out hit, layerMask))
        {
            Vector3 cameraTargetPosition = hit.point + ((hit.point - myPlayer.position)).normalized;
            smoothedPos = Vector3.Lerp(transform.position, cameraTargetPosition, Time.deltaTime * cameraSpeed);
        }
        transform.position = smoothedPos;
        transform.LookAt(myPlayer, cameraMount.transform.up);        
    }
}
