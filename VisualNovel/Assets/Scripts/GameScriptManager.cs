using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScriptManager : MonoBehaviour
{
    public GameObject stopMenu;
    public bool GamePaused = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ) 
        {
            if (!GamePaused && !stopMenu.activeSelf)
            {
                setGamePause();
                stopMenu.SetActive(true);
            }
            else if (GamePaused && stopMenu.activeSelf)
            {
                setGamePause();
                stopMenu.SetActive(false);
            }
        }
    }
    public void GoToMainMenu() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void setGamePause() 
    {
        if (GamePaused)
            GamePaused = false;
        else
            GamePaused = true;
    }
}
