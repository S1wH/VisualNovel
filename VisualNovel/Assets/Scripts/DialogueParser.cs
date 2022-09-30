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
        public string name;
        public string content;
        public int pose;

        public DialogueLine(string n, string c, int p) 
        {
            name = n;
            content = c;
            pose = p;
        }
    }

    void Start()
    {
        string file = "Dialogue";
        string sceneNum = SceneManager.GetActiveScene().buildIndex.ToString();
        file += sceneNum;
        file += ".txt";
        LoadDialogue(file);
    }

    void LoadDialogue(string fileName) 
    {
        string file = "Assets/Dialogues/" + fileName;
        string line;
        StreamReader fileReader = new StreamReader(file);
        using (fileReader) 
        {
            do 
            {
                line = fileReader.ReadLine();
                if (line != null) 
                {
                    string[] line_content = line.Split(';');
                    DialogueLine dialogueLine = new DialogueLine(line_content[0], line_content[1], int.Parse(line_content[2]));
                    Dialogue.Add(dialogueLine);
                }
            }
            while (line != null);
            fileReader.Close();
        }
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
