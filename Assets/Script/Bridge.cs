using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bridge : MonoBehaviour
{
    public static Bridge instance;

    public int sceneExplored = 0;
    public int currentScene = 0;
    // Start is called before the first frame update
    // void Awake()
    // {
    //     if (instance != null)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(instance);
    //     }
    // }
    
    // void OnApplicationQuit()
    // {
    //     Save();
    // }
    // public void UnlockLevel()
    // {
    //     currentScene = SceneManager.GetActiveScene().buildIndex;
    //     if (currentScene >= sceneExplored) sceneExplored = currentScene;
    //     Save();
    // }
    // void Save()
    // {
    //     SaveSystem.SaveSceneData(currentScene, sceneExplored);
    // }
    
}
