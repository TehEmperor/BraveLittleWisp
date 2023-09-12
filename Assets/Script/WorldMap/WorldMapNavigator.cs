using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldMapNavigator : MonoBehaviour
{    
    [SerializeField] float cameraLerpDuration = 2f;    
    [SerializeField] GameObject subject;
    List<Vector3> destinationsToReach = new List<Vector3>();
    
    public void NavigateToLevel(MapNode node)
    {
        Teleport(node.transform.position);
        onSetCamera?.Invoke(node.GetViewPoint(), cameraLerpDuration);
    }
    
    public void SetCameraView(Transform view)
    {
        onSetCamera?.Invoke(view, 1);
    }

    private void Teleport(Vector3 destination)
    {
        destinationsToReach.Add(destination);
        StartCoroutine(WaitAndGo());

    }

    IEnumerator WaitAndGo()
    {
        yield return new WaitForSeconds(cameraLerpDuration/2);
        subject.transform.position = destinationsToReach[0];
        destinationsToReach.Remove(subject.transform.position);
    }






    public delegate void SetCamera(Transform targetCameraPosition, float timeToLerp);
    public event SetCamera onSetCamera;





}
