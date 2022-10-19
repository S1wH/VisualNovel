using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    void Start()
    {
        EditorSceneManager.preventCrossSceneReferences = false;
    }

    public void StartNewGame() 
    {
        GameVariables.NewGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
