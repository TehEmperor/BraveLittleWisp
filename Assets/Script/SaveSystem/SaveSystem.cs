using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

public class SaveSystem : MonoBehaviour
{
    public void Save(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        CaptureState(state);
        SaveFile(saveFile, state);
    }

    private void SaveFile(string saveFile, Dictionary<string, object> state)
    {
        string path = GetPathFromSaveFile(saveFile);
        print("Saving to" + path);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        print("Loading from" + path);
        if (!File.Exists(path))
        {
            return new Dictionary<string, object>();
        }
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    public void Load(string saveFile)
    {
        RestoreState(LoadFile(saveFile));
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (MapNode node in FindObjectsOfType<MapNode>())
        {
            string title = node.GetLevelData().name;
            if (state.ContainsKey(title))
            {
             node.RestoreState(state[title]);
            }

        }
    }
    private void CaptureState(Dictionary<string, object> state)
    {
       foreach(MapNode node in FindObjectsOfType<MapNode>())
       {
        string title = node.GetLevelData().name;
        if(state.ContainsKey(title)){state[title] = node.CaptureState(); continue;};
        state.Add(node.GetLevelData().name, node.CaptureState());
       }
    }

    // public static void SaveSceneData(int _currentScene, int _sceneExplored)
    // {
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     string path = Application.persistentDataPath + "/Scene.json";
    //     FileStream stream = new FileStream(path, FileMode.Create);
    //     SceneData data = new SceneData(_currentScene, _sceneExplored);
    //     formatter.Serialize(stream, data);
    //     stream.Close();
    // }

    // public static SceneData LoadSceneData()
    // {
    //     string path = Application.persistentDataPath + "/Scene.json";
    //     if (File.Exists(path))
    //     {
    //         BinaryFormatter formatter = new BinaryFormatter();
    //         FileStream stream = new FileStream(path, FileMode.Open);
    //         SceneData data = formatter.Deserialize(stream) as SceneData;
    //         stream.Close();
    //         return data;
    //     }
    //     else
    //     {
    //         Debug.Log("File not found: " + path);
    //         return null;
    //     }
    // }

    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }



}
