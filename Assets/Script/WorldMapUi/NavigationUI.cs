using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NavigationUI : MonoBehaviour
{
    [SerializeField] Button currentLevelName;
    [SerializeField] Transform previousLevelsContainer;
    [SerializeField] Transform nextLevelsContainer;
    [SerializeField] Button choiceButton;

    public event Action<Level> onLevelSet; 
    
    public void SetActiveLevel(Level level)
    {
        currentLevelName.GetComponentInChildren<TextMeshProUGUI>().text = level.name;
        currentLevelName.GetComponent<LevelDescriptionSpawner>().SetLevel(level);
        SpawnLevelButtons(level.GetReachableLevels(), nextLevelsContainer);
        SpawnLevelButtons(level.GetPreviousLevels(), previousLevelsContainer);  
        onLevelSet?.Invoke(level);
    }



    private void SpawnLevelButtons(List<Level> levels, Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        foreach (var level in levels)
        {
            Button choiceB =  Instantiate(choiceButton, container);
            choiceB.GetComponentInChildren<TextMeshProUGUI>().text = level.name;
            choiceB.onClick.AddListener( () => {SetActiveLevel(level);});            
        }
    }
}
