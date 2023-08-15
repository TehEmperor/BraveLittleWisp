using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "WorldMapLayout", menuName = "Map/WorldMapLayout", order = 0)]
public class WorldMapLayout : ScriptableObject 
{
    [SerializeField] Level[] levelsMap; 

    public Level GetStartingLevel()
    {
        return levelsMap[0];
    }

    public Level[] GetLevelsMap()
    {
        return levelsMap;
    }
    
}
