using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backgrounds : MonoBehaviour
{
    public List <GameObject> backgroundImages;
    public List<string> backgroundNames;

    private void Start()
    {
        foreach (GameObject image in backgroundImages)
        {
            backgroundNames.Add(image.name);
        }
    }
}
