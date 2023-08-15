using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] musicToPlay;
    static MusicPlayer instance = null;
    AudioSource mySource;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance);

        mySource = GetComponent<AudioSource>();        
    }

    private void Start()
    {
        PlayRandomClip();
    }
    private void Update() 
    {
        
        if(!mySource.isPlaying)
        {
            PlayRandomClip();
        }
        
    }

    private void PlayRandomClip()
    {
        mySource.clip = musicToPlay[Random.Range(0, musicToPlay.Length - 1)];
        mySource.Play();
    }
}
