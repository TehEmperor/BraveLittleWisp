using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public int currentScene;
    public int sceneExplored;
    public SceneData(int _currentScene, int _sceneExplored)
    {
        currentScene = _currentScene;
        sceneExplored = _sceneExplored;
    }
}