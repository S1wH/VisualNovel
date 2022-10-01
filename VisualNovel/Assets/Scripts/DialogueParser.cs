using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class DialogueParser : MonoBehaviour
{

    List<DialogueLine> Dialogue = new List<DialogueLine>();
    struct DialogueLine
    {
        public string action;
        public string name;
        public string content;
        public int pose;

        public DialogueLine(string a, string n, string c=null, int p=-1) 
        {
            action = a;
            name = n;
            content = c;
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

    void LoadDialogue(string fileName) 
    {
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
                    DialogueLine dialogueLine;
                    string[] line_content = line.Split(';');
                    if (line_content.Length == 2)
                        dialogueLine = new DialogueLine(line_content[0], line_content[1]);
                    else
                        dialogueLine = new DialogueLine(line_content[0], line_content[1], line_content[2], int.Parse(line_content[3]));
                    Dialogue.Add(dialogueLine);
                }
            }
            while (line != null);
            fileReader.Close();
        }
    }

    public string getAction(int lineNumber)
    {
        if (lineNumber < Dialogue.Count)
            return Dialogue[lineNumber].action;
        return "";
    }
    public string getName(int lineNumber) 
    {
        if (lineNumber < Dialogue.Count)
            return Dialogue[lineNumber].name;
        return "";
    }

    public string getContent(int lineNumber)
    {
        if (lineNumber < Dialogue.Count)
            return Dialogue[lineNumber].content;
        return "";
    }

    public int getPose(int lineNumber)
    {
        if (lineNumber < Dialogue.Count)
            return Dialogue[lineNumber].pose;
        return 0;
    }
}
