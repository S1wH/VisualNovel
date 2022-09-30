using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScriptManager : MonoBehaviour
{
    public GameObject stopMenu;
    public GameObject settingsMenu;
    public bool gamePaused = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ) 
        {
            if (!gamePaused && !stopMenu.activeSelf)
            {
                setGamePause();
                stopMenu.SetActive(true);
            }
            else if (gamePaused && stopMenu.activeSelf && !settingsMenu.activeSelf)
            {
                setGamePause();
                stopMenu.SetActive(false);
            }
            else if (gamePaused && stopMenu.activeSelf && settingsMenu.activeSelf)
                settingsMenu.SetActive(false);
        }
    }
    public void GoToMainMenu() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void setGamePause() 
    {
        if (gamePaused)
            gamePaused = false;
        else
            gamePaused = true;
    }
}
