using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    public GUIStyle customStyle;
    public string dialogue;

    int lineNum;
    DialogueParser dialogueParser;
    void Start()
    {
        dialogue = "";
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        lineNum = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) 
        {
            dialogue = dialogueParser.getContent(lineNum);
            lineNum++;
        }
    }

    private void OnGUI()
    {
        dialogue = GUI.TextField(new Rect(100, 400, 600, 200), dialogue, customStyle);
    }
}
