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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
