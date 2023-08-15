using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapKeeper : MonoBehaviour
{
    [SerializeField] WorldMapLayout myLayout;
    [SerializeField] WorldMapNavigator myNavigator;
    [SerializeField] NavigationUI myUI;
    [SerializeField] float fadeOutDuration = 2f;
    [SerializeField] float fadeInDuration = 1f;
    MapNode[] nodesInScene = null;
    MainMenu myMenu;
    Level currentLevel = null;
    public static int currentMapIndex = 0;
    const string defaultSaveFile = "save";
    Dictionary<Level, MapNode> LevelToMapDictionary = new Dictionary<Level, MapNode>();

    private void Awake() 
    {
        Load();
        LevelToMapDictionary.Clear();
        LevelToMapDictionary = BuildNodeDictionary();      
        myMenu = FindObjectOfType<MainMenu>();
        myMenu.onStateSwitch += MapInitialise;
        myUI.onLevelSet += LevelChange;
        if(currentLevel == null)
        {
        currentLevel = myLayout.GetStartingLevel();
        }
    }

    private void Start()
    {
        DrawPaths();
    }

    private void DrawPaths()
    {
        foreach (MapNode node in nodesInScene)
        {
            node.DrawPossiblePaths();
        }
    }

    private void MapInitialise(Transform menuCameraPos)
    {
        if(menuCameraPos!=null)
        {
            myNavigator.SetCameraView(menuCameraPos);
        }
        else
        {            
            myUI.SetActiveLevel(currentLevel);
        }
    }  
         

    public void LevelChange(Level level)
    {
        myNavigator.NavigateToLevel(GetNodeFromLevel(level));
        currentLevel = level;
    }

   
    private Dictionary<Level, MapNode> BuildNodeDictionary()
    {
        Dictionary<Level, MapNode> sortingDict = new Dictionary<Level, MapNode>();
        nodesInScene = FindObjectsOfType<MapNode>();
        foreach (MapNode node in nodesInScene)
        {
            node.SetKeeper(this);
            sortingDict.Add(node.GetLevelData(), node);
        }

        return sortingDict;
    }

    public int GetGatheredSpirits()
    {
        int spiritsGathered = 0;
        foreach(Level level in myLayout.GetLevelsMap())
        {
            if(!level.IsFinished())continue;
            spiritsGathered+= level.GetSoulsAcquired();
        }
        return spiritsGathered;
    }

    public MapNode GetNodeFromLevel(Level level)
    {
        return LevelToMapDictionary[level];
    }
    
    public void FinishLevel(Level level)
    {
        level.Finish();        
    }

    public void DiscoverLevel(Level level)
    {
        level.Discover();        
    }

    public void RequestLevelLoad()
    {
        myMenu.SwitchState(MainMenu.State.Loading);
        StartCoroutine(LoadLevel());
    }

    public IEnumerator LoadLevel()
    {
        if (currentLevel.GetLevelReference() == "") { myMenu.SwitchState(MainMenu.State.Menu); yield break; }
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutDuration);
        Save();
        GameObject.DontDestroyOnLoad(this);        
        yield return SceneManager.LoadSceneAsync(currentLevel.GetLevelReference());
        foreach(Portal portal in FindObjectsOfType<Portal>())
        {portal.SetLevel(currentLevel);};
        yield return fader.FadeIn(fadeInDuration);
        Destroy(gameObject);
    }

    
    public void Save()
    {
        GetComponent<SaveSystem>().Save(defaultSaveFile);
    }

    public void Load()
    {
        GetComponent<SaveSystem>().Load(defaultSaveFile);
    }

    

    
}
