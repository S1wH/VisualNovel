using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class DialogueParser : MonoBehaviour
{
    private string action;

    public List<DialogueLine> Dialogue;
    public struct DialogueLine
    {
        public string action;
        public string name;
        public string content1;
        public string content2;
        public string content3;
        public string conseq1;
        public string conseq2;
        public string conseq3;
        public int time;
        public int pose;

        public DialogueLine(string a, string n = null, string c1 = null, string c2 = null, string c3 = null, string co1 = null, string co2 = null, string co3 = null, int t=0, int p = -1)
        {
            action = a;
            name = n;
            content1 = c1;
            content2 = c2;
            content3 = c3;
            conseq1 = co1;
            conseq2 = co2;
            conseq3 = co3;
            time = t;
            pose = p;
        }
    }

    void Start()
    {
        // get file with a dialogue according to scene build index
        string file = "Dialogue";
        string sceneNum = SceneManager.GetActiveScene().buildIndex.ToString();
        file += sceneNum;
        file += ".txt";
        LoadDialogue(file);
    }

    public List<DialogueLine> LoadDialogue(string fileName)
    {
        Dialogue = new List<DialogueLine>();
        // open file with a dialogue
        string file = "Assets/Dialogues/" + fileName;
        string line;
        StreamReader fileReader = new StreamReader(file);
        using (fileReader)
        {
            // read file and read content while line != null
            do
            {
                line = fileReader.ReadLine();
                if (line != null)
                {
                    DialogueLine dialogueLine = new DialogueLine();
                    string[] lineContent = line.Split(';');
                    action = lineContent[0];
                    dialogueLine.action = action;

                    // different actions according to action content
                    if (action == "say")
                    {
                        dialogueLine.name = lineContent[1];
                        dialogueLine.content1 = lineContent[2];
                        dialogueLine.pose = int.Parse(lineContent[3]);
                    }
                    else if (action == "cbg")
                    {
                        dialogueLine.content1 = lineContent[1];
                        dialogueLine.content2 = lineContent[2];
                    }
                    else if (action == "startm")
                    {
                        dialogueLine.content1 = lineContent[1];
                    }
                    else if (action == "choice")
                    {
                        dialogueLine.content1 = lineContent[1];
                        dialogueLine.content2 = lineContent[2];
                        dialogueLine.conseq1 = lineContent[3];
                        dialogueLine.conseq2 = lineContent[4];
                        if (lineContent.Length == 6)
                            dialogueLine.time = int.Parse(lineContent[5]);
                    }
                    else if (action == "interactiveChoice")
                    {
                        dialogueLine.content1 = lineContent[1];
                        dialogueLine.content2 = lineContent[2];
                        dialogueLine.content3 = lineContent[3];
                        dialogueLine.conseq1 = lineContent[4];
                        dialogueLine.conseq2 = lineContent[5];
                        dialogueLine.conseq3 = lineContent[6];
                        if (lineContent.Length == 8)
                            dialogueLine.time = int.Parse(lineContent[7]);
                    }
                    Dialogue.Add(dialogueLine);
                }
            }
            while (line != null);
            fileReader.Close();
            return Dialogue;
        }
    }
}
