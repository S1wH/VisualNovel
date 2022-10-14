using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // content inside dialogue line
    private string action;
    private string content1;
    private string content2;
    private string choice1;
    private string choice2;
    private int pose;
    private string Name;
    
    // everyting that connects with dialogue parser or dialogue line system
    private DialogueParser dialogueParser;
    public List<DialogueParser.DialogueLine> mainDialogue;
    public List<DialogueParser.DialogueLine> chosenDialogue;
    List<DialogueParser.DialogueLine> dialogueLines;

    public int lineNumMain;
    public int lineNumChosen = 0;
    private int lineN;

    // variables for text typing
    private int lettersPLaced;
    private bool textIsTyping;
    private float typingSpeed;
    private Coroutine displayLineCoroutine;

    // managers of the game
    [SerializeField] private SettingsManager SettingsManager;
    [SerializeField] private GameScriptManager GameScriptManager;
    [SerializeField] private SaveManager SaveManager;

    // dialogue panel and their components
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterText;

    void Start()
    {
        // activate dialogue panel and find parser from which we can get dialogue
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        dialoguePanel.SetActive(true);

        // check if it is a new game or not
        if (DataHolder.NewGame)
        {
            lineNumMain = 0;
            mainDialogue = dialogueParser.Dialogue;
        }
        else
        {
            GameData data = DataHolder.Data;
            mainDialogue = data.mainDialogue;
            chosenDialogue = data.choiceDialogue;
            if (chosenDialogue == null)
                lineNumMain = data.mainDialogueLine - 1;
            else
                lineNumMain = data.mainDialogueLine + 1;
            lineNumChosen = data.choiceDialogueLine - 1;
        }
        Invoke("MakeDialogueAction", 1f);
    }

    void Update()
    {
        // check if we can place new line 
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameScriptManager.gamePaused && !textIsTyping && lettersPLaced == 0) 
        {
            MakeDialogueAction();
        }

        // check if we can place all text in textbox
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameScriptManager.gamePaused && textIsTyping && lettersPLaced > 1)
        {

            GameScriptManager.setGamePause();
            StopCoroutine(displayLineCoroutine);
            textIsTyping = false;
            dialogueText.text = content1;
            lettersPLaced = 0;
            GameScriptManager.setGamePause();
        }
    }

    public void ChangeDialogue(Button button)
    {
        // select new dialogue according to player's choice
        string fileName;
        if (button.name == "Choice1")
            fileName = choice1;
        else
            fileName = choice2;

        // get new dialogue for consequences of player's choice
        chosenDialogue = dialogueParser.LoadDialogue(fileName + ".txt");
        lineNumChosen = 0;
        GameScriptManager.setGamePause();
        MakeDialogueAction();
    }

    public void ClearDialoguePanel()
    {
        dialogueText.SetText("");
        characterText.SetText("");
    }

    public void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowUpDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    private void MakeDialogueAction() 
    {
        // check if there is a choice dialogue 
        if (chosenDialogue != null)
        {
            // check if any lines left in it
            if (chosenDialogue.Count > lineNumChosen)
            {
                lineN = lineNumChosen;
                dialogueLines = chosenDialogue;
                lineNumChosen++;
            }

            // flow to main dialogue
            else
            {
                chosenDialogue = null;
                lineN = lineNumMain;
                dialogueLines = mainDialogue;
                lineNumMain++;
            }
        }

        // also flow to main dialogue
        else
        {
            lineN = lineNumMain;
            dialogueLines = mainDialogue;
            lineNumMain++;
        }

        //check if any lines left in this dialogue
        if (dialogueLines.Count > lineN)
        {
            action = dialogueLines[lineN].action;

            // actions for dialogue text file
            if (action == "say")
            {
                ParseSayLine(dialogueLines, lineN);
                PlaceText();
                lineN++;
            }
            else if (action == "cbg")
            {
                // here we also place next line 
                ParseChangeLine(dialogueLines, lineN);
                GameScriptManager.ChageBackground(content1, content2);
                lineN++;
                action = "say";
                Invoke("MakeDialogueAction", 2.5f);
            }
            else if (action == "choice")
            {
                ParseChoiceLine(dialogueLines, lineN);
                GameScriptManager.MakeChoice(content1, content2);
                lineN++;
            }
            else if (action == "startm")
            {
                ParseStartMusicLine(dialogueLines, lineN);
                GameScriptManager.StartMusic(content1);
                lineN++;
            }
            else if (action == "stopm")
            {
                GameScriptManager.StopMusic();
                lineN++;
            }
        }
    }

    // get content from one parsed line for all parse functions
    private void ParseSayLine(List<DialogueParser.DialogueLine> dialogue, int lineNum)
    {
        content1 = dialogue[lineNum].content1;
        pose = dialogue[lineNum].pose;
        Name = dialogue[lineNum].name;
    }

    private void ParseChangeLine(List<DialogueParser.DialogueLine> dialogue, int lineNum)
    {
        content1 = dialogue[lineNum].content1;
        content2 = dialogue[lineNum].content2;
    }

    private void ParseChoiceLine(List<DialogueParser.DialogueLine> dialogue, int lineNum)
    {
        content1 = dialogue[lineNum].content1;
        content2 = dialogue[lineNum].content2;
        choice1 = dialogue[lineNum].conseq1;
        choice2 = dialogue[lineNum].conseq2;
    }

    private void ParseStartMusicLine(List<DialogueParser.DialogueLine> dialogue, int lineNum)
    {
        content1 = dialogue[lineNum].content1;
    }

    private void PlaceText() 
    {
        // getting typing speed from settings
        typingSpeed = (1 - SettingsManager.typingValue) / 7;

        // getting name and color of a character
        Character character = GameObject.Find(Name).GetComponent<Character>();
        characterText.color = new Color(character.color[0], character.color[1], character.color[2]);

        // displaying it
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);
        displayLineCoroutine = StartCoroutine(DisplayLine());
        characterText.text = character.characterName;
    }

    private IEnumerator DisplayLine()
    {
        // ienumerator for typing text 
        dialogueText.text = "";
        textIsTyping = true;
        foreach (char letter in content1.ToCharArray())
        {
            lettersPLaced++;
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        lettersPLaced = 0;
        textIsTyping = false;
    }
}
