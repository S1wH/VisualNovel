using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public float typingValue;
    public float musicValue, soundValue;

    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundPref = "SoundPref";
    private static readonly string TypingPref = "TypingPref";

    private int FirstPLayInt;

    [SerializeField] private Slider musicSlider, soundSlider, typingSlider;

    public AudioSource[] musicAudio;
    public AudioSource[] soundAudio;

    void Start()
    {
        // check if it is a first player's entry into the game
        FirstPLayInt = PlayerPrefs.GetInt(DataHolder.FirstPlay);
        if (FirstPLayInt == 0) 
        {
            // set default volume settings
            musicValue = 0.25f;
            soundValue = 0.25f;
            PlayerPrefs.SetFloat(MusicPref, musicValue);
            PlayerPrefs.SetFloat(SoundPref, soundValue);
            PlayerPrefs.SetInt(DataHolder.FirstPlay, -1);

            //set default typings speed settings
            typingValue = 0.25f;
            PlayerPrefs.SetFloat(TypingPref, typingValue);
        }
        else 
        {
            // get last player's volume settings
            musicValue = PlayerPrefs.GetFloat(MusicPref);
            soundValue = PlayerPrefs.GetFloat(SoundPref);

            // ger last player's typing settings
            typingValue = PlayerPrefs.GetFloat(TypingPref);
        }

        // set this settings to sliders
        musicSlider.value = musicValue;
        soundSlider.value = soundValue;
        typingSlider.value = typingValue;
    }

    public void SaveSoundSettings() 
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundPref, soundSlider.value);
    }

    public void SaveTypingSettings()
    {
        PlayerPrefs.SetFloat(TypingPref, typingSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        // save settings when options menu is closed
        if (!focus)
        {
            SaveSoundSettings();
            SaveTypingSettings();
        }
    }

    public void UpdateSound() 
    {
        // change music and sound volume according to sliders' values 
        for (int i = 0; i < musicAudio.Length; i++)
        {
            musicAudio[i].volume = musicSlider.value;
        }
        for (int j = 0; j < soundAudio.Length; j++)
        {
            soundAudio[j].volume = soundSlider.value;
        }
    }

    public void UpdateTypingSpeed()
    {
        // change typing settings according to it's value 
        typingValue = typingSlider.value;
    }
}
