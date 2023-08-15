using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldPortal : Portal
{
    [SerializeField] int soulsOnLevel = 0;
    void Start()
    {
        fires = SpawnColumns(fireColumnPrefab, soulsOnLevel);
      //  HushFires(GetComponentInParent<Level>().IsFinished);    

    }

}
