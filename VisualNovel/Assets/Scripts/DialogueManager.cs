using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    private int lineNum;
    private string dialogue;
    private int pose;
    private string character;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    DialogueParser dialogueParser;
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
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && GameScriptManager.GamePaused == false) 
        {
            PLaceNewLine();
        }
    }

    private void PLaceNewLine() 
    {
        dialogue = dialogueParser.getContent(lineNum);
        pose = dialogueParser.getPose(lineNum);
        character = dialogueParser.getName(lineNum);
        dialogueText.text = dialogue;
        lineNum++;
    }
}
