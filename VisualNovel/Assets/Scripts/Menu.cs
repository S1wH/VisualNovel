using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject settingsMenu;

    void Start()
    {
        EditorSceneManager.preventCrossSceneReferences = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (saveMenu.activeSelf)
                saveMenu.SetActive(false);
            else if (settingsMenu.activeSelf)
                settingsMenu.SetActive(false);
        }
    }

    public void StartNewGame() 
    {
        GameVariables.NewGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
