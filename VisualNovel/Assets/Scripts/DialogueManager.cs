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

    public string dialogueNameMain;
    public string dialogueNameChoice;
    public int lineNumMain;
    public int lineNumChosen = 0;
    private int lineN;
    private string dialogueNameNow;

    public float delayChangeAction = 2f;
    public float delayFade = 0.05f;

    // variables for text typing
    private int lettersPLaced;
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
        GameVariables.isSkipping = false;
        typingSpeed = (1 - SettingsManager.typingValue) / 7;
        // activate dialogue panel and find parser from which we can get dialogue
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        dialoguePanel.SetActive(true);

        // check if it is a new game or not
        if (GameVariables.NewGame)
        {
            lineNumMain = 0;
            dialogueNameMain = "Dialogue1";
            mainDialogue = dialogueParser.Dialogue;
        }
        else
        {
            GameData data = DataHolder.Data;
            mainDialogue = data.mainDialogue;
            chosenDialogue = data.choiceDialogue;
            dialogueNameMain = data.mainDialogueName;
            dialogueNameChoice = data.chosenDialogueName;
            if (chosenDialogue == null)
                lineNumMain = data.mainDialogueLine - 1;
            else
                lineNumMain = data.mainDialogueLine + 1;
            lineNumChosen = data.choiceDialogueLine - 1;
        }
        SaveManager.LoadSeenActions();
        if (DataHolder.Data != null && DataHolder.Data.SeenDialogues != null)
        {
            DataHolder.SeenDialogues = DataHolder.Data.SeenDialogues;
            DataHolder.SeenDialoguesLines = DataHolder.Data.SeenDialoguesLines;
        }
        else
        {
            DataHolder.SeenDialogues = new List<string>();
            DataHolder.SeenDialoguesLines = new List<int>();
        }
        Invoke("MakeDialogueAction", 0.5f);
    }

    void Update()
    {
        // check if we can place new line 
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameVariables.GamePaused && !GameVariables.TextIsTyping && lettersPLaced == 0 && !GameVariables.actionHappens) 
        {
            MakeDialogueAction();
        }

        // check if we can place all text in textbox
        else if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameVariables.GamePaused && GameVariables.TextIsTyping && lettersPLaced > 1 && GameVariables.actionHappens)
        {

            GameScriptManager.setGamePause();
            StopCoroutine(displayLineCoroutine);
            GameVariables.actionHappens = false;
            GameVariables.TextIsTyping = false;
            dialogueText.text = content1;
            lettersPLaced = 0;
            GameScriptManager.setGamePause();
        }

        // check if user holds tab to skip all actions
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameVariables.isSkipping) 
                ActionsArentSkipping();
            else
                ActionsAreSkipping();
        }
        else if (!GameVariables.actionHappens && GameVariables.isSkipping && !GameVariables.GamePaused)
            MakeDialogueAction();
    }

    public void ChangeDialogue(Button button)
    {
        // select new dialogue according to player's choice
        string fileName;
        if (button.name == "Choice1")
            fileName = choice1;
        else
            fileName = choice2;
        dialogueNameChoice = fileName;

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
                dialogueNameNow = dialogueNameChoice;
            }

            // flow to main dialogue
            else
            {
                chosenDialogue = null;
                dialogueNameChoice = null;
                lineN = lineNumMain;
                dialogueLines = mainDialogue;
                dialogueNameNow = dialogueNameMain;
                lineNumMain++;
            }
        }

        // also flow to main dialogue
        else
        {
            lineN = lineNumMain;
            dialogueLines = mainDialogue;
            dialogueNameNow = dialogueNameMain;
            lineNumMain++;
        }
        bool seen = CheckLineIfItWasSeen(dialogueNameNow, lineN);
        if (!seen)
        {
            ActionsArentSkipping();
        }
        //check if any lines left in this dialogue
        if (dialogueLines.Count > lineN)
        {
            action = dialogueLines[lineN].action;
            GameVariables.actionHappens = true;

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
                StartCoroutine("MakeNextAction");
            }
            else if (action == "choice")
            {
                Debug.Log('a');
                ParseChoiceLine(dialogueLines, lineN);
                GameScriptManager.MakeChoice(content1, content2);
                lineN++;
                GameVariables.actionHappens = false;
            }
            else if (action == "startm")
            {
                ParseStartMusicLine(dialogueLines, lineN);
                GameScriptManager.StartMusic(content1);
                lineN++;
                GameVariables.actionHappens = false;
                StartCoroutine("MakeNextAction");
            }
            else if (action == "stopm")
            {
                GameScriptManager.StopMusic();
                lineN++;
                GameVariables.actionHappens = false;
                StartCoroutine("MakeNextAction");
            }
        }
    }

    private bool CheckLineIfItWasSeen(string name, int line)
    {
        foreach (string dialogueName in DataHolder.SeenDialogues)
        {
            if (dialogueName == name)
            {
                if (DataHolder.SeenDialoguesLines[DataHolder.SeenDialogues.IndexOf(name)] >= line)
                {
                    return true;
                }
                else
                {
                    DataHolder.SeenDialoguesLines[DataHolder.SeenDialogues.IndexOf(name)] = line;
                    return false;
                }
            }
        }
        DataHolder.SeenDialogues.Add(name);
        DataHolder.SeenDialoguesLines.Add(line);
        return false;
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
        // getting name and color of a character
        Character character = GameObject.Find(Name).GetComponent<Character>();
        characterText.color = new Color(character.color[0], character.color[1], character.color[2]);

        // displaying it
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);
        displayLineCoroutine = StartCoroutine(DisplayLine());
        characterText.text = character.characterName;
    }

    private void ActionsAreSkipping()
    {
        GameVariables.isSkipping = true;
        typingSpeed = 0.01f;
        delayChangeAction = 0.1f;
        delayFade = 0.005f;
    }

    private void ActionsArentSkipping()
    {
        GameVariables.isSkipping = false;
        typingSpeed = (1 - SettingsManager.typingValue) / 7;
        delayChangeAction = 2f;
        delayFade = 0.05f;
    }

    private IEnumerator DisplayLine()
    {
        // ienumerator for typing text 
        dialogueText.text = "";
        GameVariables.TextIsTyping = true;
        foreach (char letter in content1.ToCharArray())
        {
            lettersPLaced++;
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        lettersPLaced = 0;
        GameVariables.TextIsTyping = false;
        GameVariables.actionHappens = false;
    }

    private IEnumerator MakeNextAction()
    {
        yield return new WaitForSeconds(delayChangeAction);
        GameVariables.actionHappens = false;
        MakeDialogueAction();
    }
}
