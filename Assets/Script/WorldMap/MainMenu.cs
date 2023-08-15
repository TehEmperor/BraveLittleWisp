using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform cameraMenu;    
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject navMenuUi;
    State state;
    public enum State 
    {
        Menu,
        Map,
        Loading
    }
    public delegate void OnSwitchState(Transform menuCameraPos); //0 for menu, 1 for map;
    public event OnSwitchState onStateSwitch;
    public event Action<int> onMove;
    
   

    private void Start()
    {
        SwitchState(State.Menu);
    }
    
    private void Update() {

        switch (state)
        {
            default:
            case State.Menu:
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchState(State.Map);
            }
                            
                break;
            case State.Map:
               
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SwitchState(State.Menu);
                }

                
                if(Input.GetKeyDown(KeyCode.R))//for debug;
                {
                    //keeper.ResetMap();
                }
                break;
            case State.Loading:
            
                break;    

        }        
    }

    public void SwitchState(State _state)
    {
        if(_state == State.Menu)
        {
            onStateSwitch?.Invoke(cameraMenu);
            mainMenuUI.SetActive(true);
            navMenuUi.SetActive(false);
        }
        
        if(_state == State.Map)
        {
            onStateSwitch?.Invoke(null);
            mainMenuUI.SetActive(false);
            navMenuUi.SetActive(true);
        }
       
        state = _state;
    }

    public void Play()
    {
        SwitchState(State.Map);
    }
    public void Exit()
    {
        Application.Quit();
    }

}
