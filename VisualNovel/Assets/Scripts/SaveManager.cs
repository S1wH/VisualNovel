using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameScriptManager gameManager;
    [SerializeField] private List<Button> saveSlots;

    private Storage storage;
    private char id;

    private static readonly string SavePref = "SavePref";

    public string mode;

    private void Start()
    {
        // check if it is a first play
        if (PlayerPrefs.GetInt(DataHolder.FirstPlay) == 0)
        {
            // set all save slots to empty
            foreach (Button saveSlot in saveSlots)
            {
                PlayerPrefs.SetInt(SavePref + saveSlot.name[saveSlot.name.Length - 1], 0);
            }
        }
        else
        {
            int val;
            foreach (Button saveSlot in saveSlots)
            {
                // set save slots
                val = PlayerPrefs.GetInt(SavePref + saveSlot.name[saveSlot.name.Length - 1]);
                if (val != 0)
                    saveSlot.GetComponentInChildren<Text>().text = "Занято";
            }
        }
    }

    public void SaveGame(Button button)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText.text == "Занято" || buttonText.text == "Пусто")
            id = button.name[button.name.Length - 1];

        // if there is a save in this slot
        if (buttonText.text == "Занято")
        {
            string text = "Вы действительно хотите перезаписать данное сохранение?\nДанные этого сохранения будут утрачены";
            gameManager.AreYouSure(text);
            return;
        }

        // if there is no save in this slot
        else if (buttonText.text == "Пусто")
        {
            storage = new Storage();
            GameData data = gameManager.GetData();
            PlayerPrefs.SetInt(SavePref + id, 1);
            storage.Save(data, id);
            buttonText.text = "Занято";
        }

        // player doesn't want to save game in a locked slot
        else
        {
            gameManager.CloseSureBox();
            storage = new Storage();
            GameData data = gameManager.GetData();
            storage.Save(data, id);
        }
    }

    public void LoadGame(Button button)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText.text == "Занято")
            id = button.name[button.name.Length - 1];

        // there is a save in this slot
        if (buttonText.text != "Пусто" || buttonText.text != "Да")
        {
            storage = new Storage();
            object data;

            // if we want to load while playing a game
            if (SceneManager.GetActiveScene().buildIndex != 0 && buttonText.text == "Занято")
            {
                string text = "Вы действительно хотите загрузить сохранение?\nВсе несохраненные данные будут утеряны";
                id = button.name[button.name.Length - 1];
                gameManager.AreYouSure(text);
                return;
            }
            else
                data = storage.Load(id);
            DataHolder.Data = (GameData)data;
            GameVariables.NewGame = false;

            // load from main menu
            if (buttonText.text == "Занято")
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // load while playing a game
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public static void SaveSeenActions()
    {
        Storage storage = new Storage();
        GameData gameData = new GameData
        {
            SeenDialogues = DataHolder.SeenDialogues,
            SeenDialoguesLines = DataHolder.SeenDialoguesLines
        };
        storage.Save(gameData, 'A');
    }

    public static void LoadSeenActions()
    {
        Storage storage = new Storage();
        object data = storage.Load('A');
        DataHolder.Data = (GameData)data;
    }

    public void ChooseAction(Button button)
    {
        // check if player want to save or load a game
        if (mode == "SaveMode")
            SaveGame(button);
        else
            LoadGame(button);
    }

    public void SetMode(Button button)
    {
        // set mode according to button text
        if (button.name == "SaveBtn")
            mode = "SaveMode";
        else
            mode = "LoadMode";
    }
}
