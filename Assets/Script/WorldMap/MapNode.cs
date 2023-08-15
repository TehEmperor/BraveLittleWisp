using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapNode : MonoBehaviour, ISaveable
{
    [SerializeField] Level levelData;
    [SerializeField] Line linePrefab;
    [SerializeField] Transform viewPoint;
    WorldMapKeeper myKeeper;
    [System.Serializable]
    class LevelData
    {
        public LevelData(string n, bool d, bool f)
        {
            this.name = n;
            this.discoverd = d;
            this.finished = f;
            
        }
        string name;
        public bool discoverd;
        public bool finished;
    }


    public void SetKeeper(WorldMapKeeper keeper)
    {
        myKeeper = keeper;
    }
    
    public Transform GetViewPoint()
    {
        return viewPoint;
    }

    public Level GetLevelData()
    {
        return levelData;
    }

    public void DrawPossiblePaths()
    {        
        foreach(var node in GetDestinationsFromLevels(levelData.GetReachableLevels()))
        {
            Line pathRenderer = Instantiate(linePrefab, gameObject.transform);
            pathRenderer.LineSetUp(BuildPath(transform.position, node));
        }
        
    }

    private List<Vector3> GetDestinationsFromLevels(List<Level> levels)
    { 
        List<Vector3> reacheableLevels = new List<Vector3> ();
        foreach (var level in levels)
        {  
            reacheableLevels.Add(myKeeper.GetNodeFromLevel(level).transform.position);
        }
        return reacheableLevels;
    }

    private List<Vector3> BuildPath(Vector3 from, Vector3 to)
    {
        List<Vector3> navigationPath = new List<Vector3>();
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
        navigationPath.AddRange(path.corners);
        return navigationPath;
    }

    public object CaptureState()
    {
        LevelData saveable = new LevelData(levelData.name, levelData.IsDiscovered(), levelData.IsFinished());     
        return saveable;   
    }

    public void RestoreState(object state)
    {
        print("TryingToLoad");
        LevelData restoreData = (LevelData)state;
        if(!restoreData.discoverd) {levelData.ResetToDefaultState(); return;}
        if(restoreData.finished){levelData.Finish();}
        if(restoreData.discoverd){levelData.Discover();}
        
    }
}
