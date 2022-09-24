using UnityEngine;
using UnityEngine.Audio;
using System;
using System.IO;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    static string path = "SaveInfo/SaveInfo.txt";
    StreamReader file = new StreamReader(path);

    public void SetVolume(float volume)
    {
        string info = file.ReadLine();


        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
}
