using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameScriptManager : MonoBehaviour
{
    private List<GameObject> backs;
    private List<string> backsNames;

    private GameObject bg;
    private GameObject newBg;

    [SerializeField] private GameObject stopMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject choiceBox;
    [SerializeField] private Button Choice1;
    [SerializeField] private Button Choice2;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Backgrounds backgrounds;

    public bool gamePaused = false;

    private void Start()
    {
        // get all backgrounds and their names
        backs = backgrounds.backgroundImages;
        backsNames = backgrounds.backgroundNames;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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

    public void setGamePause()
    {
        // set game pause status
        if (gamePaused)
            gamePaused = false;
        else
            gamePaused = true;
    }

    public void MakeChoice(string choice1, string choice2)
    {
        setGamePause();
        Choice1.GetComponentInChildren<TextMeshProUGUI>().text = choice1;
        Choice2.GetComponentInChildren<TextMeshProUGUI>().text = choice2;
        choiceBox.SetActive(true);

    }

    public void ChangeMusic(string name1, string name2)
    {

    }

    public void ChageBackground(string name1, string name2)
    {
        // set game to pause and get names of an old a new backs
        setGamePause();
        bg = backs[backsNames.IndexOf(name1)];
        newBg = backs[backsNames.IndexOf(name2)];

        // clear dialogue panel
        dialogueManager.ClearDialoguePanel();
        dialogueManager.HideDialoguePanel();

        // here we check if a number of a layer of new bg is more or less than the old one
        // it matters because of a visibility of UI in a scene
        // we have to know if the new bg is higher in the hierarchy than the old one

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

        // invoke scripts for showing panel ans setting game to play mode 
        Invoke("ShowDialoguePanel", 2f);
        Invoke("setGamePause", 2f);
    }

    private void ShowDialoguePanel()
    {
        //here we show up dialogue panel
        ChangeColorAlpha(0, bg.GetComponent<Image>());
        dialogueManager.ShowUpDialoguePanel();
    }

    private void ChangeColorAlpha(float val, Image im)
    {
        // changinf of alpha of color
        Color color = im.color;
        color.a = val;
        im.color = color;
    }

    IEnumerator FadeDown(Image bgImage)
    {
        // ienumerator for fading down image
        for (float f = 1f; f > 0; f -= 0.05f)
        {
            ChangeColorAlpha(f, bgImage);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeUp(Image bgImage)
    {
        // ienumerator for fading up image
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            ChangeColorAlpha(f, bgImage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
