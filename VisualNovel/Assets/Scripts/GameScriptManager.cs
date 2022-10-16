using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameScriptManager : MonoBehaviour
{
    private List<GameObject> musicList;
    private List<GameObject> backs;
    private List<string> backsNames;
    private List<string> musicNames;

    private Image bgImage;
    private Image newBgImage;

    private AudioSource music;

    [SerializeField] private GameObject sureBox;
    [SerializeField] private GameObject stopMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject choiceBox;
    [SerializeField] private Button Choice1;
    [SerializeField] private Button Choice2;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Collections collections;

    public bool gamePaused = false;

    private void Start()
    {
        collections.InitializeCollections();
        // get all backgrounds and their names
        backs = collections.backgroundImages;
        backsNames = collections.backgroundNames;
        //get all muisc and their names
        musicList = collections.music;
        musicNames = collections.musicNames;

        if (!DataHolder.NewGame)
        {
            GameData data = DataHolder.Data;
            newBgImage = backs[backsNames.IndexOf(data.activeBackgroundName)].GetComponent<Image>();
            music = musicList[musicNames.IndexOf(data.activeMusicName)].GetComponent<AudioSource>();
        }
        else
        {
            music = musicList[0].GetComponent<AudioSource>();
            newBgImage = backs[0].GetComponent<Image>();
        }
        music.Play();
        ChangeColorAlpha(1, newBgImage);
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
            // game isn't paused and stop menu isn't activated
            if (!gamePaused && !stopMenu.activeSelf)
            {
                setGamePause();
                stopMenu.SetActive(true);
            }

            // game is paused, stop menu is activated, but settings menu and save menu aren't activated
            else if (gamePaused && stopMenu.activeSelf && !settingsMenu.activeSelf && !saveMenu.activeSelf)
            {
                setGamePause();
                stopMenu.SetActive(false);
            }
            
            // game is paused, stop menu and settings menu are activated
            else if (gamePaused && stopMenu.activeSelf && settingsMenu.activeSelf)
                settingsMenu.SetActive(false);

            // game is paused, stop menu  and save menu are activated
            else if (gamePaused && stopMenu.activeSelf && saveMenu.activeSelf)
                saveMenu.SetActive(false);
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
        // activate choice box
        setGamePause();
        Choice1.GetComponentInChildren<TextMeshProUGUI>().text = choice1;
        Choice2.GetComponentInChildren<TextMeshProUGUI>().text = choice2;
        choiceBox.SetActive(true);

    }

    public void StopMusic()
    {
        StartCoroutine("LowMusic");
    }

    public void StartMusic(string musicName)
    {
        music = musicList[musicNames.IndexOf(musicName)].GetComponent<AudioSource>();
        music.volume = 0;
        music.Play();
        StartCoroutine("HighMusic");
    }

    public void ChageBackground(string name1, string name2)
    {
        // set game to pause and get names of an old a new backs
        setGamePause();
        GameObject bg = backs[backsNames.IndexOf(name1)];
        GameObject newBg = backs[backsNames.IndexOf(name2)];
        bgImage = bg.GetComponent<Image>();
        newBgImage = newBg.GetComponent<Image>();
        // clear dialogue panel
        dialogueManager.ClearDialoguePanel();
        dialogueManager.HideDialoguePanel();

        // here we check if a number of a layer of new bg is more or less than the old one
        // it matters because of a visibility of UI in a scene
        // we have to know if the new bg is higher in the hierarchy than the old one
        if (newBg.layer > bg.layer)
        {
            StartCoroutine("FadeUp");
        }
        else
        {
            ChangeColorAlpha(1, newBgImage);
            StartCoroutine("FadeDown");
        }

        // invoke scripts for showing panel ans setting game to play mode 
        Invoke("ShowDialoguePanel", dialogueManager.delayChangeAction);
        Invoke("setGamePause", dialogueManager.delayChangeAction);
    }

    public void AreYouSure(string text)
    {
        // activate sure box
        sureBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
        sureBox.SetActive(true);
    }

    public void CloseSureBox()
    {
        sureBox.SetActive(false);
    }

    private void ShowDialoguePanel()
    {
        //here we show up dialogue panel
        ChangeColorAlpha(0, bgImage);
        dialogueManager.ShowUpDialoguePanel();
    }

    private void ChangeColorAlpha(float val, Image im)
    {
        // changinf of alpha of color
        Color color = im.color;
        color.a = val;
        im.color = color;
    }

    IEnumerator FadeDown()
    {
        // ienumerator for fading down image
        for (float f = 1f; f > 0; f -= 0.05f)
        {
            ChangeColorAlpha(f, bgImage);
            yield return new WaitForSeconds(dialogueManager.delayFade);
        }
    }

    IEnumerator FadeUp()
    {
        // ienumerator for fading up image
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            ChangeColorAlpha(f, newBgImage);
            yield return new WaitForSeconds(dialogueManager.delayFade);
        }
    }

    IEnumerator HighMusic()
    {
        // ienumerator for making volume higher
        for (float i = 0f; i <= settingsManager.musicValue; i += 0.005f)
        {
            music.volume = i;
            yield return new WaitForSeconds(0.005f);
        }
        music.volume = settingsManager.musicValue;
    }
    IEnumerator LowMusic()
    {
        // ienumerator for making volume lower
        for (float i = music.volume; i > 0 ; i -= 0.005f)
        {
            music.volume = i;
            yield return new WaitForSeconds(0.005f);
        }
        music.volume = 0;
    }

    public GameData GetData()
    {   
        // getting data for save file
        GameData gameData = new GameData();
        gameData.mainDialogue = dialogueManager.mainDialogue;
        gameData.choiceDialogue = dialogueManager.chosenDialogue;
        gameData.mainDialogueLine = dialogueManager.lineNumMain;
        gameData.choiceDialogueLine = dialogueManager.lineNumChosen;
        gameData.activeBackgroundName = newBgImage.name;
        gameData.activeCharacters = null;
        gameData.activeMusicName = music.name;
        return gameData;
    }
}
