using UnityEngine;

public class DataHolder : MonoBehaviour
{
    private static readonly string SavePref = "SavePref";
    public static int savePrefInt;
    public static bool NewGame { get; set; }

    private void Start()
    {
        savePrefInt = PlayerPrefs.GetInt(SavePref);
    }

    public static void SetSavePref()
    {
        savePrefInt++;
        PlayerPrefs.SetInt(SavePref, savePrefInt);
    }
}
