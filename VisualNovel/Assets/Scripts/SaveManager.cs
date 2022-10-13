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
        if (PlayerPrefs.GetInt(DataHolder.FirstPlay) == 0)
        {
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
                val = PlayerPrefs.GetInt(SavePref + saveSlot.name[saveSlot.name.Length - 1]);
                if (val != 0)
                    saveSlot.GetComponentInChildren<Text>().text = "������";
            }
        }
    }

    public void SaveGame(Button button)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText.text == "������" || buttonText.text == "�����")
            id = button.name[button.name.Length - 1];
        if (buttonText.text == "������")
        {
            string text = "�� ������������� ������ ������������ ������ ����������?\n������ ����� ���������� ����� ��������";
            gameManager.AreYouSure(text);
            return;
        }
        else if (buttonText.text == "�����")
        {
            storage = new Storage();
            GameData data = gameManager.GetData();
            PlayerPrefs.SetInt(SavePref + id, 1);
            storage.Save(data, id);
            buttonText.text = "������";
        }
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
        if (buttonText.text == "������")
            id = button.name[button.name.Length - 1];
        if (buttonText.text != "�����" || buttonText.text != "��")
        {
            storage = new Storage();
            object data;
            if (SceneManager.GetActiveScene().buildIndex != 0 && buttonText.text == "������")
            {
                string text = "�� ������������� ������ ��������� ����������?\n��� ������������� ������ ����� �������";
                id = button.name[button.name.Length - 1];
                gameManager.AreYouSure(text);
                return;
            }
            else
                data = storage.Load(id);
            DataHolder.Data = (GameData)data;
            DataHolder.NewGame = false;
            if (buttonText.text == "������")
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("There is no save here");
        }
    }

    public void ChooseAction(Button button)
    {
        if (mode == "SaveMode")
            SaveGame(button);
        else
            LoadGame(button);
    }

    public void SetMode(Button button)
    {
        if (button.name == "SaveBtn")
            mode = "SaveMode";
        else
            mode = "LoadMode";
    }
}
