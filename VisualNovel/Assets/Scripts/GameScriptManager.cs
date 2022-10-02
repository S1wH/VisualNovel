using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScriptManager : MonoBehaviour
{
    [SerializeField] private GameObject stopMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private DialogueManager dialogueManager;

    public bool gamePaused = false;
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
        GameObject bg = GameObject.Find(names[0]);
        GameObject newBg = GameObject.Find(names[1]);
        dialogueManager.ClearDialoguePanel();
        dialogueManager.HideDialoguePanel();
        if (newBg.layer > bg.layer)
        {
            Image newBgImage = newBg.GetComponent<Image>();
            StartCoroutine("FadeUp", newBgImage);
        }
        else
        {
            Image bgImage = bg.GetComponent<Image>();
            StartCoroutine("FadeDown", bgImage);
        }
        Invoke("ShowDialoguePanel", 2f);
        Invoke("setGamePause", 2f);
    }

    private void ShowDialoguePanel()
    {
        dialogueManager.ShowUpDialoguePanel();
    }

    IEnumerator FadeDown(Image bgImage)
    {
        for (float f = 1f; f > 0; f -= 0.05f)
        {
            Color color = bgImage.color;
            color.a = f;
            bgImage.color = color;
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
