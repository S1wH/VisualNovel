using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    private string filePath;

    public int SaveAmount
    {
        get
        {
            return saveAmount;
        }
    }

    public Storage()
    {
        filePath = Application.persistentDataPath + "/saves/GameSave" + DataHolder.savePrefInt + ".save";
        DataHolder.SetSavePref();
    }
}
