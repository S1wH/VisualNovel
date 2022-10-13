using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DialogueLineSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var dialogueLine = (DialogueParser.DialogueLine)obj;
        info.AddValue(name:"action", dialogueLine.action);
        info.AddValue(name: "name", dialogueLine.name);
        info.AddValue(name: "content1", dialogueLine.content1);
        info.AddValue(name: "content2", dialogueLine.content2);
        info.AddValue(name: "conseq1", dialogueLine.conseq1);
        info.AddValue(name: "conseq2", dialogueLine.conseq2);
        info.AddValue(name: "pose", dialogueLine.pose);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var dialogeLine = (DialogueParser.DialogueLine)obj;
        dialogeLine.action = (string)info.GetValue(name: "action", typeof(string));
        dialogeLine.name = (string)info.GetValue(name: "name", typeof(string));
        dialogeLine.content1 = (string)info.GetValue(name: "content1", typeof(string));
        dialogeLine.content2 = (string)info.GetValue(name: "content2", typeof(string));
        dialogeLine.conseq1 = (string)info.GetValue(name: "conseq1", typeof(string));
        dialogeLine.conseq2 = (string)info.GetValue(name: "conseq2", typeof(string));
        dialogeLine.pose = (int)info.GetValue(name: "pose", typeof(int));
        obj = dialogeLine;
        return obj;
    }
}
