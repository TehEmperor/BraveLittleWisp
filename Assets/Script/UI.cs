using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, background, level;
    [SerializeField] Button levelButton, level_2, level_3;
    int sceneToLoad = 0;

    private void Start()
    {

        //You can comment it out for Debug; 
        level_2.interactable = false; //Level 2 and 3 button are not interactive at the beginning; 
        level_3.interactable = false;
        levelButton.interactable = false;
        //Load();
        var _sceneExplored = Bridge.instance.sceneExplored;
        if (_sceneExplored > 1) levelButton.interactable = true;
    }
    public void Play()
    {
        LoadLevel(Bridge.instance.currentScene > 1 ? Bridge.instance.currentScene : 1);
        //var scene = Bridge.instance.currentScene + 1;
        //var scene = Bridge.instance.currentScene;
        //if (scene > 0) LoadLevel(Bridge.instance.currentScene);
        //else LoadLevel(1);
    }
    public void Exit()
    {
        Application.Quit();
    }

    // Level button activator; Also activate/deactivate button based on the level been explored; 
    public void Level()
    {

        //Bridge.instance.UnlockLevel();
        var _sceneExplored = Bridge.instance.sceneExplored;
        level.SetActive(true);
        mainMenu.SetActive(false);
        if (_sceneExplored > 3)
        {
            level_3.interactable = true;
        }
        if (_sceneExplored > 2)
        {
            level_2.interactable = true;
        }
    }

    // They are assigned to each level selector buttons
    public void Level_1()
    {
        LoadLevel(2);
        //sceneToLoad = 1;
    }
    public void Level_2()
    {
        LoadLevel(3);
        //sceneToLoad = 2;
    }
    public void Level_3()
    {
        LoadLevel(4);
        //sceneToLoad = 3;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Escape from main menu will case game exit; 
        {
            Exit(); //Only works in Main Menu; 
        }
    }
    void LoadLevel(int _level)
    {
        SceneManager.LoadScene(_level);
    }

    // void Load()
    // {
    //     SceneData data = SaveSystem.LoadSceneData();
    //     if (data != null)
    //     {
    //         Debug.Log("Loaded Data: " + data.currentScene + " " + data.sceneExplored);
    //         Bridge.instance.currentScene = data.currentScene;
    //         Bridge.instance.sceneExplored = data.sceneExplored;
    //     }
    //     else
    //     {
    //         Debug.Log("No Data To Load");
    //     }

    // }

}


