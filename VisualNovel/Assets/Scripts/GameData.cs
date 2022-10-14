using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    // info that is saved in a default save
    public List<DialogueParser.DialogueLine> mainDialogue;
    public List<DialogueParser.DialogueLine> choiceDialogue;
    public List<Character> activeCharacters;

    public int mainDialogueLine;
    public int choiceDialogueLine;

    public string activeBackgroundName;
    public string activeMusicName;
}
