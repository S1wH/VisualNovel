using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameData
{
    public List<DialogueParser.DialogueLine> mainDialogue;
    public List<DialogueParser.DialogueLine> choiceDialogue;
    public int mainDialogueLine;
    public int choiceDialogueLine;

    public List<Character> activeCharacters;

    public string activeBackgroundName;
    public string activeMusicName;
}
