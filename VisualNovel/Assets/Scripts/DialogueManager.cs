using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private int lineNumMain;
    private int lineNumChosen = 0;
    private int lineN;

    private string action;
    private string content1;
    private string content2;
    private string choice1;
    private string choice2;
    private int pose;
    private string Name;
        
    private DialogueParser dialogueParser;
    private List<DialogueParser.DialogueLine> mainDialogue;
    private List<DialogueParser.DialogueLine> chosenDialogue;

    // variables for text typing
    private int lettersPLaced;
    private bool textIsTyping;
    private float typingSpeed;
    private Coroutine displayLineCoroutine;

    // managers of the game
    [SerializeField] private SettingsManager SettingsManager;
    [SerializeField] private GameScriptManager GameScriptManager;

    // dialogue panel and their components
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterText;

    void Start()
    { 
        // set dialogue panel to true and get dialogue parser
        dialoguePanel.SetActive(true);
        content1 = "";
        lineNumMain = 0;
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        mainDialogue = dialogueParser.Dialogue;
    }

    void Update()
    {
        // check if we can place new line 
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameScriptManager.gamePaused && !textIsTyping && lettersPLaced == 0) 
        {
            MakeDialogueAction();
        }

        // check if we have to place all text in textbox
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
        string fileName;
        if (button.name == "Choice1")
            fileName = choice1;
        else
            fileName = choice2;
        chosenDialogue = dialogueParser.LoadDialogue(fileName + ".txt");
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
        List<DialogueParser.DialogueLine> dialogueLines;
        if (chosenDialogue != null)
        {
            if (chosenDialogue.Count > lineNumChosen)
            {
                lineN = lineNumChosen;
                dialogueLines = chosenDialogue;
                lineNumChosen++;
            }
            else
            {
                lineN = lineNumMain;
                dialogueLines = mainDialogue;
                lineNumMain++;
            }
        }
        else
        {
            lineN = lineNumMain;
            dialogueLines = mainDialogue;
            lineNumMain++;
        }
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
            else if (action == "cm")
            {
                ParseChangeLine(dialogueLines, lineN);
                GameScriptManager.ChangeMusic(content1, content2);
                lineN++;
            }
        }
    }

    private void ParseSayLine(List<DialogueParser.DialogueLine> dialogue, int lineNum)
    {
        // get content from one parsed line with say action
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
