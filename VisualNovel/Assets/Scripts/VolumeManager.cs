using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundPref = "SoundPref";
    private int FirstPLayInt;
    public Slider musicSlider, soundSlider;
    private float musicValue, soundValue;
    public AudioSource[] musicAudio;
    public AudioSource[] soundAudio;

    void Start()
    {
        FirstPLayInt = PlayerPrefs.GetInt(FirstPlay);
        if (FirstPLayInt == 0) 
        {
            musicValue = 0.25f;
            soundValue = 0.25f;
            PlayerPrefs.SetFloat(MusicPref, musicValue);
            PlayerPrefs.SetFloat(SoundPref, soundValue);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else 
        {
            musicValue = PlayerPrefs.GetFloat(MusicPref);
            soundValue = PlayerPrefs.GetFloat(SoundPref);
        }
        musicSlider.value = musicValue;
        soundSlider.value = soundValue;
    }

    public void SaveSoundSettings() 
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundPref, soundSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SaveSoundSettings();
    }

    public void UpdateSound() 
    {
        for (int i = 0; i < musicAudio.Length; i++)
        {
            musicAudio[i].volume = musicSlider.value;
        }
        for (int j = 0; j < soundAudio.Length; j++)
        {
            soundAudio[j].volume = soundSlider.value;
        }
    }
}
