using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    private int lineNum;
    private string dialogue;
    private int pose;
    private string characterName;

    private DialogueParser dialogueParser;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterText;
    public GameScriptManager GameScriptManager;

    void Start()
    { 
        dialoguePanel.SetActive(true);
        dialogue = "";
        lineNum = 0;
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && GameScriptManager.gamePaused == false) 
        {
            PLaceNewLine();
        }
    }

    private void PLaceNewLine() 
    {
        dialogue = dialogueParser.getContent(lineNum);
        pose = dialogueParser.getPose(lineNum);
        characterName = dialogueParser.getName(lineNum);
        if (dialogue != "" && characterName != "")
        {
            Character character = GameObject.Find(characterName).GetComponent<Character>();
            characterText.color = new Color(character.color[0], character.color[1], character.color[2]);
            dialogueText.text = dialogue;
            characterText.text = character.characterName;
            lineNum++;
        }
    }
}
