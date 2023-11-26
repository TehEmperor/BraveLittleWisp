using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endMenu;
    
    //, optionMenu;

   
    private void OnEnable() {
        PlayerController.onLastRights += EndGame;
    }
    private void OnDisable() 
    {
        PlayerController.onLastRights -= EndGame;

       
       

    }
    public void MainMenu()
    {
        //Bridge.instance.UnlockLevel();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Option()
    {
        //optionMenu.SetActive(true);
    }
    void Pause()
    {
        if(!pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            return;
        }
        else if(pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        

       
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void VolumeChange(float value)
    {
        //Volume stuff, doesn't do anything for now
        //Sound.volume = value;
    }
    void Update()
    {
        //Debug.Log("Current Scene " + SceneManager.GetActiveScene().buildIndex); //Debug.

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    void EndGame()
    {
        Time.timeScale = 0;
        endMenu.SetActive(true);

    }


    public void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

  

}
