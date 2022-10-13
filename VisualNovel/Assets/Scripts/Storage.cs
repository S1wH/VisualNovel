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
        formatter = new BinaryFormatter();
        var selector = new SurrogateSelector();

        var dialogeLineSurrogate = new DialogueLineSerializationSurrogate();
        selector.AddSurrogate(typeof(DialogueParser.DialogueLine), new StreamingContext(StreamingContextStates.All), dialogeLineSurrogate);

        formatter.SurrogateSelector = selector;
    }

    private void CreateFilePath(char id)
    {
        var directory = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        filePath = directory + "/GameSave" + id + ".save";

    }
    public object Load(char id)
    {
        CreateFilePath(id);
        var file = File.Open(filePath, FileMode.Open);
        var savedData = formatter.Deserialize(file);
        file.Close();
        return savedData;
    }

    public void Save(object saveData, char id)
    {
        CreateFilePath(id);
        var file = File.Create(filePath);
        formatter.Serialize(file, saveData);
        file.Close();
    }
}
