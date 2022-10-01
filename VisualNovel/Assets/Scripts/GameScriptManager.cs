using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScriptManager : MonoBehaviour
{
    public GameObject stopMenu;
    public GameObject settingsMenu;

    public bool gamePaused = false;

    public Animator animator;

    void Update()
    {
        // different activities when esc button is pressed
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

    public void ChageBackground(string name)
    {
        Image bgImage = GameObject.Find("BackGroundImage").GetComponent<Image>();
        Sprite newBgImage = Resources.Load<Sprite>("Sprites/Backgrounds/MainMenu");
        animator.Play("ChangeBackgrouns");
        bgImage.overrideSprite = newBgImage;
    }

    public void setGamePause() 
    {
        // set game pause status
        if (gamePaused)
            gamePaused = false;
        else
            gamePaused = true;
    }
}
