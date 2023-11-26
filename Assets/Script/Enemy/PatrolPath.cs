using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    const float wayPointGizmoRadius = 0.3f;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawSphere(GetWayPoint(i).position, wayPointGizmoRadius);
            Gizmos.DrawLine(GetWayPoint(i).position, GetWayPoint(GetNextIndex(i)).position);


        }

    }
    public int GetWaypointsAmount()
    {
        return transform.childCount;
    }

    public int GetNextIndex(int i)
    {
        if (i + 1 == transform.childCount)
        {
            return 0;
        }
        else return i + 1;

    }

    public Transform GetWayPoint(int i)
    {
        return transform.GetChild(i);
    }
}
