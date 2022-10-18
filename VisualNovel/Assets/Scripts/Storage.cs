using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Storage
{
    private BinaryFormatter formatter;
    private string filePath;
    public Storage()
    {
        InitBinaryFormatter();
    }

    private void InitBinaryFormatter() 
    {
        // intitialize formatter that can serialize and deserialize structures
        formatter = new BinaryFormatter();
        var selector = new SurrogateSelector();
        var dialogeLineSurrogate = new DialogueLineSerializationSurrogate();

        // add surrogate of DialogueLine
        selector.AddSurrogate(typeof(DialogueParser.DialogueLine), new StreamingContext(StreamingContextStates.All), dialogeLineSurrogate);
        formatter.SurrogateSelector = selector;
    }

    private void CreateFilePath(char id)
    {
        // creatinf a filepath according to id of pressed button
        var directory = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        filePath = directory + "/GameSave" + id + ".save";

    }
    public object Load(char id)
    {
        // here we create file path, open it, get deserialized data from it and return
        CreateFilePath(id);
        if (File.Exists(filePath))
        {
            var file = File.Open(filePath, FileMode.Open);
            var savedData = formatter.Deserialize(file);
            file.Close();
            return savedData;
        }
        else
            return null;
    }

    public void Save(object saveData, char id)
    {
        // here we create file path, create file and serialize data into it
        CreateFilePath(id);
        var file = File.Create(filePath);
        formatter.Serialize(file, saveData);
        file.Close();
    }
}
