using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScriptManager : MonoBehaviour
{
    public GameObject stopMenu;
    private bool GamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            GamePaused = true;
            stopMenu.SetActive(true);
        }
    }
}
