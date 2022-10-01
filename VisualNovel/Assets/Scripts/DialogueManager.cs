using System.Collections;
using System.Collections.Generic;
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
    }

    void Update()
    {
        // check if we can place new line 
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && GameScriptManager.gamePaused == false) 
        {
            ParsNewLine();
            MakeDialogueAction();
        }
    }

    private void MakeDialogueAction() 
    {
        if (action == "say")
            PlaceText();
        else if (action == "cbg")
            GameScriptManager.ChageBackground(Name);
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
        Character character = GameObject.Find(Name).GetComponent<Character>();
        characterText.color = new Color(character.color[0], character.color[1], character.color[2]);
        dialogueText.text = dialogue;
        characterText.text = character.characterName;
        lineNum++;
    }
}
