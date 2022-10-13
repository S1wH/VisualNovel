using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collections : MonoBehaviour
{
    public List <GameObject> backgroundImages;
    public List<GameObject> music;
    public List<string> backgroundNames;
    public List<string> musicNames;

    public void InitializeCollections()
    {
        foreach (GameObject image in backgroundImages)
        {
            backgroundNames.Add(image.name);
        }
        foreach (GameObject music in music)
        {
            musicNames.Add(music.name);
        }
    }
}
