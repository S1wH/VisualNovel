using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DataHolder
{
    public static readonly string FirstPlay = "FirstPlay";
    public static bool NewGame { get; set; }
    public static GameData Data { get; set; }
    public static List<string> SeenDialogues { get; set; }
    public static List<int> SeenDialoguesLines { get; set; }
}
