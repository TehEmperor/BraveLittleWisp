using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldMapNavigator : MonoBehaviour
{    
    [SerializeField] float cameraLerpDuration = 2f;    
    [SerializeField] NavMeshAgent agent;

    
    public void NavigateToLevel(MapNode node)
    {
        agent.destination = node.transform.position;
        onSetCamera?.Invoke(node.GetViewPoint(), cameraLerpDuration);
    }
    
    public void SetCameraView(Transform view)
    {
        onSetCamera?.Invoke(view, 1);
    }




    public delegate void SetCamera(Transform targetCameraPosition, float timeToLerp);
    public event SetCamera onSetCamera;





}
