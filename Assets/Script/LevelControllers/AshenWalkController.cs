using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshenWalkController : MonoBehaviour
{
    [SerializeField] Chimney myChimney;
    [SerializeField] Enemie pursuer;
    [SerializeField] float pursuerSpeedMod = 0.5f;
    Coroutine routine;
    List<Vector3> followPath = new List<Vector3>();
    private void Awake() {
        myChimney.onEmberPlaced += UpdateRoamForPursuer;
    }

    private void UpdateRoamForPursuer(Vector3 whereTo)
    {
        followPath.Add(whereTo);        
        if(routine == null)
        {
            List<Vector3> currentPath = new List<Vector3>();
            currentPath.AddRange(followPath);
            routine = StartCoroutine(WaitAndGo(currentPath));
        }              
    }

    IEnumerator WaitAndGo(IEnumerable<Vector3> path)
    {
        foreach (var dest in path)
        {
            Coroutine routine;
            routine = StartCoroutine(ReachDestination(dest));
            yield return routine;
        }
        routine = null;
    }

    IEnumerator ReachDestination(Vector3 dest)
    {
        pursuer.RoamingUpdate(dest, pursuerSpeedMod);
        while(Vector3.Distance(pursuer.transform.position, dest) > 1f)
        {
            yield return null;
        }
        followPath.Remove(dest);
    }
}
