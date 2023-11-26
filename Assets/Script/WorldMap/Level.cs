using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Map/Level", order = 1)]
public class Level : ScriptableObject
{
    [SerializeField] List<Level> previousLevels = new List<Level>();
    [SerializeField] List<Level> nextLevels = new List<Level>();
    [SerializeField] bool isFinished = false;
    [SerializeField] bool isDiscovered = true;
    [SerializeField] bool isSpecial = false;
    [SerializeField] string levelDescription;
    [Header("must be accurate and same as scene name in build index")]
    [SerializeField] string levelSceneReference; //must be accurate and same as scene name in build index
    [Header("0 is Souls To Finish, 1 is Souls Collected Total")]
    [SerializeField] int[] soulsTrack = new int[2]; //0 is Souls to activate portal, 1 is souls collected;
    
    public bool IsFinished()
    {
        return isFinished;
    }
    public bool IsDiscovered()
    {
        return isDiscovered;
    }

    public void Discover()
    {
        isDiscovered = true;
    }

    public void Finish(int soulsCollected)
    {
        isFinished = true;
        if(soulsCollected>soulsTrack[1]) soulsTrack[1] = soulsCollected;        
    }

    public int GetSoulsAcquired()
    {
        if(!isFinished) return 0;
        if(soulsTrack[1]>soulsTrack[0]) return soulsTrack[1];
        return soulsTrack[0];
    }

    public int GetSoulsToActivate()
    {
        return soulsTrack[0];
    }

    public List<Level> GetReachableLevels()
    {
        List<Level> reacheableLevels = new List<Level>();
        foreach (var level in nextLevels)
        {
            if (!level.IsDiscovered()) continue;
            if (!this.IsFinished()) continue;
            reacheableLevels.Add(level);
        }
        return reacheableLevels;
    }

    public void DiscoverNextLevels(bool secret)
    {
        if(nextLevels.Count == 0) return;
        foreach (var level in nextLevels)
        {
            if(!secret && level.isSpecial) continue;
            level.Discover();            
        }
    }

    public List<Level> GetPreviousLevels()
    {
        return previousLevels;
    }

    public void ResetToDefaultState()
    {
        isFinished = false;
        isDiscovered = false;
    }

    public string GetLevelReference()
    {
        return levelSceneReference;
    }

    public string GetLevelDescription()
    {
        return levelDescription;
    }
    
}