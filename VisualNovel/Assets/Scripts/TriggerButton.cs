using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] private Button btn;
    private void OnMouseOver()
    {
        btn.GetComponentInChildren<TextMeshProUGUI>().fontSize = 55;
    }

    private void OnMouseExit()
    {
        btn.GetComponentInChildren<TextMeshProUGUI>().fontSize = 40;
    }
}
