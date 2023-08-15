using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCameraController : MonoBehaviour
{
    WorldMapNavigator myNavigator;
    MainMenu myMenu;
    Coroutine cameraRoutine = null;

    private void Awake() {
        myNavigator = FindObjectOfType<WorldMapNavigator>();
        
    }

    private void OnEnable() 
    {
        myNavigator.onSetCamera += SetCameraPosition;
        
    }
    
    private void OnDisable()
    {
        myNavigator.onSetCamera -= SetCameraPosition;
    }
    
    public void SetCameraPosition(Transform target, float desiredDuration)
    {
        StopAllCoroutines();
        cameraRoutine = null;
        cameraRoutine = StartCoroutine(LerpPositionAndRotation(target.position, target.rotation, desiredDuration));
                       

    }
    
    

    IEnumerator LerpPositionAndRotation(Vector3 endPos, Quaternion endRot, float duration)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        while(time<duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time/duration);
            transform.rotation = Quaternion.Lerp(startRot, endRot, time/duration);
            time += Time.deltaTime;            
            yield return null;
        }
        transform.rotation = endRot;
        transform.position = endPos;
    }
}
