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
    [SerializeField] string levelSceneReference; //must be accurate and same as scene name in build index
    [SerializeField] int soulsAquired;
    
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

    public void Finish()
    {
        isFinished = true;
    }

    public int GetSoulsAcquired()
    {
        return soulsAquired;
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

    
}