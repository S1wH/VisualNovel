using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private int lineNum;

    private string action;
    private string dialogue;
    private int pose;
    private string Name;
        
    private DialogueParser dialogueParser;

    private int lettersPLaced;
    private bool textIsTyping;
    private float typingSpeed;
    private Coroutine displayLineCoroutine;

    [SerializeField] private SettingsManager SettingsManager;
    [SerializeField] private GameScriptManager GameScriptManager;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterText;

    void Start()
    { 
        // set dialogue panel to true and get dialogue parser
        dialoguePanel.SetActive(true);
        dialogue = "";
        lineNum = 0;
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();

        // set typing speed to settings value
    }

    void Update()
    {
        // check if we can place new line 
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameScriptManager.gamePaused && !textIsTyping) 
        {
            ParsNewLine();
            MakeDialogueAction();
        }
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && !GameScriptManager.gamePaused && textIsTyping && lettersPLaced > 1)
        {
            StopCoroutine(displayLineCoroutine);
            textIsTyping = false;
            dialogueText.text = dialogue;
            lettersPLaced = 0;
        }
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
        if (action == "say")
            PlaceText();
        else if (action == "cbg")
            GameScriptManager.ChageBackground(Name);
        lineNum++;
    }

    private void ParsNewLine() 
    {
        // get content from one parsed line 
        action = dialogueParser.getAction(lineNum);
        dialogue = dialogueParser.getContent(lineNum);
        pose = dialogueParser.getPose(lineNum);
        Name = dialogueParser.getName(lineNum);
    }

    private void PlaceText() 
    {
        // place text inside text box
        typingSpeed = (1 - SettingsManager.typingValue) / 7;
        Character character = GameObject.Find(Name).GetComponent<Character>();
        characterText.color = new Color(character.color[0], character.color[1], character.color[2]);
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);
        displayLineCoroutine = StartCoroutine(DisplayLine());
        characterText.text = character.characterName;
    }

    private IEnumerator DisplayLine()
    {
        dialogueText.text = "";
        textIsTyping = true;
        foreach (char letter in dialogue.ToCharArray())
        {
            lettersPLaced++;
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        lettersPLaced = 0;
        textIsTyping = false;
    }
}
