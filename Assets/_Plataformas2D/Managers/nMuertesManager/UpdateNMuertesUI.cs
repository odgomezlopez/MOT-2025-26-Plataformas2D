using System;
using TMPro;
using UnityEngine;

public class UpdateNMuertesUI : MonoBehaviour
{
    TextMeshProUGUI nMuertesText;
     void Awake()
    {
        nMuertesText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.nMuertes.OnValueChanged += UpdateNMuertes;
            UpdateNMuertes(GameData.Instance.nMuertes.Value);
        }
    }

    private void OnDisable()
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.nMuertes.OnValueChanged -= UpdateNMuertes;
        }
    }

    private void UpdateNMuertes(int obj)
    {
        nMuertesText.text = obj.ToString();
    }
}
