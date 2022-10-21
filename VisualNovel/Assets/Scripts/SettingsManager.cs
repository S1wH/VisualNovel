using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public float typingValue;
    public float musicValue, soundValue;
    public string skipMode;

    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundPref = "SoundPref";
    private static readonly string TypingPref = "TypingPref";
    private static readonly string SkipPref = "SkipPref";

    private int FirstPLayInt;

    [SerializeField] private Slider musicSlider, soundSlider, typingSlider;
    [SerializeField] private Button AllSkipBtn, SeenSkipBtn;

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

            // set default typings speed settings
            typingValue = 0.25f;
            PlayerPrefs.SetFloat(TypingPref, typingValue);

            // set default skipping settings
            SeenSkipBtn.image.color = new Color(76, 200, 20);
            SeenSkipBtn.interactable = false;
            skipMode = "seen";
        }
        else 
        {
            // get last player's volume settings
            musicValue = PlayerPrefs.GetFloat(MusicPref);
            soundValue = PlayerPrefs.GetFloat(SoundPref);

            // get last player's typing settings
            typingValue = PlayerPrefs.GetFloat(TypingPref);

            // get last player's skip mode
            skipMode = PlayerPrefs.GetString(SkipPref); 
        }

        // set this settings to sliders and buttons
        musicSlider.value = musicValue;
        soundSlider.value = soundValue;
        typingSlider.value = typingValue;

        if (skipMode == "all")
        {
            AllSkipBtn.image.color = new Color(76, 200, 20);
            AllSkipBtn.interactable = false;
        }
        else
        {
            SeenSkipBtn.image.color = new Color(76, 200, 20);
            SeenSkipBtn.interactable = false;
        }
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

    public void SaveSkipSettings()
    {
        PlayerPrefs.SetString(SkipPref, skipMode);
    }

    private void OnApplicationFocus(bool focus)
    {
        // save settings when options menu is closed
        if (!focus)
        {
            SaveSoundSettings();
            SaveTypingSettings();
            SaveSkipSettings();
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

    public void UpdateSkipMode(Button button)
    {
        // change skip setting when button is pressed
        if (button.name == "SeenBtn")
        {
            skipMode = "Seen";

            SeenSkipBtn.interactable = false;
            AllSkipBtn.interactable = true;

            SeenSkipBtn.image.color = new Color(76, 200, 20);
            AllSkipBtn.image.color = new Color(255, 255, 255);
        }
        else
        {
            skipMode = "All";

            AllSkipBtn.interactable = false;
            SeenSkipBtn.interactable = true;

            AllSkipBtn.image.color = new Color(76, 200, 20);
            SeenSkipBtn.image.color = new Color(255, 255, 255);
        }
    }
}
