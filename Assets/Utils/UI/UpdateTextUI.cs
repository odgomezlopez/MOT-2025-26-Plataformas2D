using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class UpdateTextUI : MonoBehaviour
{
    TextMeshProUGUI textUI;

    [SerializeField] Color enableColor = Color.black;
    [SerializeField] Color disableColor = Color.gray;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateText(string text)
    {
        textUI.SetText(text);
    }

    public void UpdateText(string text, bool enable)
    {
        UpdateText(text);

        if (enable) textUI.color = enableColor;
        else textUI.color = disableColor;
    }

    public void UpdateText(float value)
    {
        textUI.SetText(value.ToString()); ;//Si se quiere un formato se puede dar en los parametros de toString
    }
}
