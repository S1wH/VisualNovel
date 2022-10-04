using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameScriptManager : MonoBehaviour
{
    private List<GameObject> backs;
    private List<string> backsNames;

    private GameObject bg;
    private GameObject newBg;

    [SerializeField] private GameObject stopMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Backgrounds backgrounds;

    public bool gamePaused = false;

    private void Start()
    {
        backs = backgrounds.backgroundImages;
        backsNames = backgrounds.backgroundNames;
    }
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

    public void setGamePause()
    {
        // set game pause status
        if (gamePaused)
            gamePaused = false;
        else
            gamePaused = true;
    }

    public void ChageBackground(string name)
    {
        setGamePause();
        string[] names = name.Split(" ");
        bg = backs[backsNames.IndexOf(names[0])];
        newBg = backs[backsNames.IndexOf(names[1])];
        dialogueManager.ClearDialoguePanel();
        dialogueManager.HideDialoguePanel();
        if (newBg.layer > bg.layer)
        {
            Image newBgImage = newBg.GetComponent<Image>();
            StartCoroutine("FadeUp", newBgImage);
        }
        else
        {
            ChangeColorAlpha(1, newBg.GetComponent<Image>());
            Image bgImage = bg.GetComponent<Image>();
            StartCoroutine("FadeDown", bgImage);
        }
        Invoke("ShowDialoguePanel", 2f);
        Invoke("setGamePause", 2f);
    }

    private void ShowDialoguePanel()
    {
        ChangeColorAlpha(0, bg.GetComponent<Image>());
        dialogueManager.ShowUpDialoguePanel();
    }

    private void ChangeColorAlpha(float val, Image im)
    {
        Color color = im.color;
        color.a = val;
        im.color = color;
    }

    IEnumerator FadeDown(Image bgImage)
    {
        for (float f = 1f; f > 0; f -= 0.05f)
        {
            ChangeColorAlpha(f, bgImage);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeUp(Image bgImage)
    {
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            Color color = bgImage.color;
            color.a = f;
            bgImage.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
