using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] StatsComponent stats;
    [SerializeField] Image HPImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Buscamos la imagen
        if(HPImage == null) HPImage = GetComponent<Image>();

        //Buscamos los stats
        if (stats == null) stats = GetComponentInParent<StatsComponent>();
        if (stats == null) stats = FindFirstObjectByType<PlayerController>()?.GetComponent<StatsComponent>();

        //Si tengo stats inicializo el HP
        if (stats) UpdateHP(stats.stats.HP.Value, stats.stats.HP.MaxValue);
    }

    //Me suscribo/desuscribo de los cambio de HP
    private void OnEnable()
    {
        if (stats) stats.stats.HP.OnValueChanged += UpdateHP;
    }

    private void OnDisable()
    {
        if (stats) stats.stats.HP.OnValueChanged -= UpdateHP;
    }
    //Encargado de actualizar HP
    private void UpdateHP(float current, float max, float oldValue=0)
    {
        if(max!=0) HPImage.fillAmount = current/max;
    }
}
