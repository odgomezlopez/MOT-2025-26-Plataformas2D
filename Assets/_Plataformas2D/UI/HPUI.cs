using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    Stats stats;
    [SerializeField] Image HPImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Buscamos la imagen
        if(HPImage == null) HPImage = GetComponent<Image>();

        //Buscamos los stats
        if (stats == null) stats = GetComponentInParent<IStatsComponent>()?.Stats ?? null;
        if (stats == null) stats = FindFirstObjectByType<PlayerController>()?.GetComponent<IStatsComponent>().Stats;

        //Si tengo stats inicializo el HP
        if (stats != null) UpdateHP(stats.HP.Value, stats.HP.MaxValue);
    }

    //Me suscribo/desuscribo de los cambio de HP
    private void OnEnable()
    {
        if (stats != null) stats.HP.OnValueChanged += UpdateHP;
    }

    private void OnDisable()
    {
        if (stats != null) stats.HP.OnValueChanged -= UpdateHP;
    }
    //Encargado de actualizar HP
    private void UpdateHP(float current, float max, float oldValue=0)
    {
        if(max!=0) HPImage.fillAmount = current/max;
    }
}
