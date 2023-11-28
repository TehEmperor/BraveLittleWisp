using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField]LineRenderer lineRenderer;

    
    public void LineSetUp(List<Vector3> nodes)
    {
        Vector3[] positions = nodes.ToArray();
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
    
}
